import csv
import json
import pathlib
import pickle

def main():
    folder = pathlib.Path(__file__).parent.resolve()
    csv_path = folder / 'SJArchipelago_Tables - Map IDs.csv'
    with open(csv_path) as file:
        reader = csv.DictReader(file)
        data = [i for i in reader]
    for i in data:
        i["Difficulty"] = int(i["Difficulty"])
        i.pop("Mod")
        i.pop("Prefix")
        i.pop("Map File Name")
    mapsid_dict = {i["Map SID"]: i for i in data}
    mapname_dict = {i["Stylized Map Name"]: i for i in data}

    # this goes to the client
    json_path = folder / 'SJArchipelago_Tables - MapIDs.json'
    with open(json_path, 'w') as file:
        json.dump(mapsid_dict, file, indent=3)

    # this goes to the apworld
    pickle_path = folder / 'SJArchipelago_Tables - MapIDs.pickle'
    with open(pickle_path, "wb") as file:
        pickle.dump(mapname_dict, file)


if __name__ == '__main__':
    main()