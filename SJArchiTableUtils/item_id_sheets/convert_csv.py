import csv
import json
import pathlib
import re
import pickle

def main():
    folder = pathlib.Path(__file__).parent.resolve()
    csv_path = folder / 'SJArchipelago_Tables - Item IDs.csv'
    with open(csv_path) as file:
        reader = csv.DictReader(file)
        items = [i for i in reader if i["name"]]
       

    new_list = []
    for i in items:
        name = i["name"]
        split_caps = re.findall("[A-Z][^A-Z]*", name)
        if len(split_caps) > 0:
            new_name = " ".join(split_caps)
        i["name"] = new_name
        new_list.append(i)

    map_name_to_dict = {i["name"]: i for i in new_list}
    json_path = folder / 'SJArchipelago_Tables - ItemIDs.json'

    for i in map_name_to_dict.values():
        i.pop("Item Enum Name CopyPaste")
        i.pop("name")

    with open(json_path, 'w') as file:
        file.write(json.dumps(map_name_to_dict, indent=3))

    print(map_name_to_dict)
    pickle_path = folder / 'SJArchipelago_Tables - ItemIDs.pickle'
    with open(pickle_path, 'wb') as file:
        pickle.dump(map_name_to_dict, file)


if __name__ == '__main__':
    main()