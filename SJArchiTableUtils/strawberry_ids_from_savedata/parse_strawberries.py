import xml.etree.ElementTree as ET

def main():
    tree = ET.parse("strawberry_ids_from_savedata/28-modsavedata.celeste")
    beginner = tree.find(".//LevelSetStats[@Name='StrawberryJam2021/1-Beginner']")
    intermediate = tree.find(".//LevelSetStats[@Name='StrawberryJam2021/2-Intermediate']")
    advanced = tree.find(".//LevelSetStats[@Name='StrawberryJam2021/3-Advanced']")
    expert = tree.find(".//LevelSetStats[@Name='StrawberryJam2021/4-Expert']")
    grandmaster = tree.find(".//LevelSetStats[@Name='StrawberryJam2021/5-Grandmaster']")
    lobbies = [beginner, intermediate, advanced, expert, grandmaster]
    allberries = []
    for lobby in lobbies:
        areas = lobby.find("./Areas")
        # iterating through maps
        for area in areas.findall("AreaStats"):
            sid = area.attrib['SID']
            berries = area.findall(".//Strawberries")
            # iterating through berries in each map
            for berryset in berries:
                # there are 3 berrysets per area, one for a, b, and c side, but we just get the entity ids from the one that has entries.
                # b and c sides are mostly unused for modded, so itll mostly be in the a side.
                if len(berryset) > 0:
                    allberries.extend([f"{sid}=^.^={i.attrib['Key']}\n" for i in berryset.findall(".//EntityID")])
    with open("strawberry_ids_from_savedata/strawberries.txt", 'w') as file:
        file.writelines(allberries)

    


if __name__ == '__main__':
    main()