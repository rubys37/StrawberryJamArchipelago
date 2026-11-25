import csv
import json
import pathlib

def main():
    folder = pathlib.Path(__file__).parent.resolve()
    csv_path = folder / 'SJArchipelago_Tables - Map IDs.csv'
    with open(csv_path) as file:
        reader = csv.DictReader(file)
        map_dict = {i['Map SID']: i for i in reader}
    for j in map_dict.values():
        j["Difficulty"] = int(j["Difficulty"])
    json_path = folder / 'SJArchipelago_Tables - MapIDs.json'
    with open(json_path, 'w') as file:
        file.write(json.dumps(map_dict, indent=3))


if __name__ == '__main__':
    main()