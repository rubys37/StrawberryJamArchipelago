using System;
using System.Collections.Generic;
using Celeste.Mod.SJArchipelago.MapMods;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monocle;
using MonoMod.Cil;

namespace Celeste.Mod.SJArchipelago.Archipelago;

// TO BE CLEAR: I DID NOT WRITE (most of) THIS CODE
// I adopted this from rhelmot's bingoclient. I did have to modify it to work as a static method, more so like everything else written in this mod.
// https://github.com/rhelmot/CelesteBingoClient/blob/main/BingoClient/BingoChat.cs

public class ArchipelagoChat {
    private const float ChatTime = 4f;
    private static List<string> ChatMessages = new List<string>();
    private static List<string> ChatHistory = new List<string>();
    private static List<float> ChatTimers = new List<float>();

    public static bool ChatOpen;
    private static string Buffer = "";
    private static bool Underscore;
    private static float UnderscoreCounter;
    private static bool InhibitOne;

    public static void Load()
    {
        IL.Monocle.Engine.Update += HookUpdateEarly;
        On.Monocle.Engine.RenderCore += Render;
    }

    public static void Unload()
    {
        IL.Monocle.Engine.Update -= HookUpdateEarly;
        On.Monocle.Engine.RenderCore -= Render;

    }
    
    private static void HookUpdateEarly(ILContext il) {
        var cursor = new ILCursor(il);
        if (!cursor.TryGotoNext(MoveType.After, insn => insn.MatchCall(typeof(MInput), "Update"))) {
            throw new Exception("Could not find patch point");
        }

        cursor.EmitDelegate(Update);
    }

    private static bool gameSawNothing;
    private static void Update() {
        var gameSeesNothing = ChatOpen;
        if (gameSawNothing) {
            UneatInput();
        }

        if (gameSeesNothing) {
            EatInput();
        }
        gameSawNothing = gameSeesNothing;
        
        lock (ChatMessages) {
            for (int i = 0; i < ChatTimers.Count; i++) {
                ChatTimers[i] += Engine.RawDeltaTime;
                if (ChatTimers[i] > ChatTime) {
                    ChatTimers.RemoveAt(i);
                    ChatMessages.RemoveAt(i);
                    i--;
                }
            }
        }

        if (ChatOpen) {
            if (Input.ESC.Pressed) {
                InhibitOne = true;
                Engine.Scene.OnEndOfFrame += () => {
                    TextInput.OnInput -= OnInput;
                    ChatOpen = false;
                    InhibitOne = false;
                };
            }
        } else {
            if (SJArchipelagoModule.Settings.OpenChat.Pressed &&
                !Engine.Commands.Open) {
                TextInput.OnInput += OnInput;
                ChatOpen = true;
                InhibitOne = true;
                Engine.Scene.OnEndOfFrame += () => InhibitOne = false;
            }
        }

        UnderscoreCounter += Engine.RawDeltaTime;
        while (UnderscoreCounter >= 0.5f) {
          UnderscoreCounter -= 0.5f;
          Underscore = !Underscore;
        }
    }
    
    private static KeyboardState personalKeyboard;
    private static GamePadState[] personalGamepads = {default, default, default, default};
    private static MouseState personalMouse;
    private static void EatInput() {
        // prevent game from seeing any inputs
        // but keep a copy so we can get accurate pressed/released data for ourselves
        personalKeyboard = MInput.Keyboard.CurrentState;
        personalMouse = MInput.Mouse.CurrentState;
        for (var i = 0; i < 4; i++) {
            personalGamepads[i] = MInput.GamePads[i].CurrentState;
        }

        if (Input.ESC.Pressed)
        {
            return;
        }
        MInput.UpdateNull();
    }

    private static void UneatInput() {
        MInput.Keyboard.PreviousState = personalKeyboard;
        MInput.Mouse.PreviousState = personalMouse;
        for (var i = 0; i < 4; i++) {
            MInput.GamePads[i].PreviousState = personalGamepads[i];
        }
    }

    private static void OnInput(char ch) {
        if (InhibitOne) {
            InhibitOne = false;
            return;
        }

        if (!ChatOpen) {
            return;
        }

        Underscore = false;
        UnderscoreCounter = 0f;
        if (ch == '\r' || ch == '\n') {
            if (Buffer != "") {
                HandleChat(Buffer);
                Buffer = "";
            }
        } else if (ch == '\b') {
            if (Buffer.Length > 0) {
                Buffer = Buffer.Substring(0, Buffer.Length - 1);
            }
        } else if (!char.IsControl(ch)) {
            Buffer += ch;
        }
    }

    private static void Render(On.Monocle.Engine.orig_RenderCore orig, Engine self) {
        
        orig(self);
        if (Dialog.Language == null || ActiveFont.Font == null || ActiveFont.Font.Sizes.Count == 0) {
            return;
        }
        
        Draw.SpriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            Engine.ScreenMatrix);

        var currentBase = 1080f - 50f;
        var scale = 0.7f;
        var nLines = 0;
        lock (ChatMessages) {
            var chatTexts = ChatOpen ? ChatHistory : ChatMessages;
            var chatTimers = ChatOpen ? null : ChatTimers;
            for (int i = chatTexts.Count - 1; i >= 0 && currentBase > 0; i--) {
                if (nLines >= 5 && !ChatOpen) {
                    ChatTimers[i] = Math.Max(ChatTimers[i], ChatTime - 0.5f);
                }
                var timer = chatTimers?[i] ?? (ChatTime / 2f);
                var text = chatTexts[i];
                var alpha = timer < 0.25f ? timer * 4f : timer > (ChatTime - 0.5f) ? (ChatTime - timer) * 2 : 1f;
                var rise = timer < 0.25f ? timer * 4f : 1f;

                var textSize = ActiveFont.Measure(text) * scale;
                var lines = new List<string>();
                if (textSize.X > 1880f) {
                    while (!string.IsNullOrEmpty(text)) {
                        var textSplit = new List<string>(text.Split(' '));
                        textSplit.Reverse();
                        text = "";
                        while (textSplit.Count > 0) {
                            var maybetext = text + ' ' + textSplit[textSplit.Count - 1];
                            if (ActiveFont.Measure(maybetext).X * scale > 1880f) {
                                break;
                            }
                            text = maybetext;
                            textSplit.RemoveAt(textSplit.Count - 1);
                        }

                        if (text == "") {
                            text = textSplit[textSplit.Count - 1];
                            textSplit.RemoveAt(textSplit.Count - 1);
                        }

                        lines.Add(text);
                        textSplit.Reverse();
                        text = string.Join(" ", textSplit);
                    }
                } else {
                    lines.Add(text);
                }

                for (var j = lines.Count - 1; j >= 0; j--) {
                    ActiveFont.DrawOutline(lines[j], new Vector2(1920f - 20, currentBase), new Vector2(1f, rise), Vector2.One * scale, Color.White * alpha, 2f, Color.Black * alpha);
                    currentBase -= textSize.Y * (j == 0 ? 1.1f : 0.9f) * rise;
                    nLines++;
                }
            }
        }

        if (ChatOpen) {
            var uch = Underscore ? "_" : "";
            var prompt = "> " + Buffer;
            var width = ActiveFont.Measure(prompt).X * scale;
            if (width > 1920f - 20f) {
                ActiveFont.DrawOutline(prompt, new Vector2(1920f - 10f, 1080f - 10f), new Vector2(1f, 1f), Vector2.One * scale, Color.White, 2f, Color.Black);
                ActiveFont.DrawOutline(uch, new Vector2(1920f - 10f, 1080f - 10f), new Vector2(0f, 1f), Vector2.One * scale, Color.White, 2f, Color.Black);
            } else {
                ActiveFont.DrawOutline(prompt, new Vector2(10, 1080f - 10f), new Vector2(0f, 1f), Vector2.One * scale, Color.White, 2f, Color.Black);
                ActiveFont.DrawOutline(uch, new Vector2(10 + width, 1080f - 10f), new Vector2(0f, 1f), Vector2.One * scale, Color.White, 2f, Color.Black);
            }
        }
        Draw.SpriteBatch.End();
    }

    public static void Chat(string text) {
        lock (ChatMessages) {
            ChatHistory.Add(text);
            ChatMessages.Add(text);
            ChatTimers.Add(0f);
        }
    }
    
    private static void HandleChat(string msg)
    {
        APConnData conn;
        if (msg.Length != 0 && msg[0] == '/') {
            var args = msg.Split(' ');
            switch (args[0]) {
                case "/connect":
                    if (SJArchipelagoModule.SaveData == null)
                    {
                        Chat("Connection Failed. Please connect after selecting a save file.");
                        break;
                    }

                    if (args.Length != 4 && args.Length != 3)
                    {
                        Chat("Connection Failed. The command syntax is /connect [url:port] [slot name] [password(optional)]");
                        break;
                    }

                    var uri = args[1];
                    var slotName = args[2];
                    var password = "";
                    if (args.Length == 4)
                    {
                        password = args[3];
                    } 
                    conn = new APConnData(uri, slotName, password);
                    if (!APClient.ConnectToAPServer(conn).Successful)
                    {
                        Chat("Bad Connect. Parameters are invalid, try again.");
                        break;
                    }
                    Chat($"Connected Successfully to {conn.Uri} with slot {conn.SlotName}");
                    break;

                case "/reconnect":
                    if (SJArchipelagoModule.SaveData == null)
                    {
                        Chat("Connection Failed. Please connect after selecting a Strawberry Jam Archipelago save file.");
                        break;
                    }
                    conn = APConnData.FromJson(SJArchipelagoModule.SaveData.ConnectionData);
                    if (!APClient.ConnectToAPServer(conn).Successful)
                    {
                        Chat("Bad Connect. Saved parameters are invalid, This could mean the port changed or the server is down. Retry, or use /connect to change the parameters.");
                        break;
                    }
                    Chat($"Connected Successfully to {conn.Uri} with slot {conn.SlotName}");
                    break;
                
                case "/resync":
                    // testing command
                    SJArchipelagoModule.SaveData.RequiredHearts[1] = 10;
                    MapEntranceMod.MapUnlockHandler();
                    break;

                default:
                    Chat("Bad command. Valid commands are: /connect, /reconnect");
                    break;
            }
        } else {
            Chat(msg);
        }
    }
}