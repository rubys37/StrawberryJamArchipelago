import csv
import json
import pathlib
import pickle

def main():
    current_path = pathlib.Path(__file__).parent.resolve()
    csv_file = current_path / "SJArchipelago_Tables - Locations Sheet.csv"
    with open(csv_file) as file:
        reader = csv.DictReader(file)
        data = [i for i in reader]
    sid_to_apworldid = {i["Entity ID"]: int(i["APWorld Location ID"]) for i in data}
    hrn_to_dict = {i["Human Readable Name"]: i for i in data}

    SID_to_APworldIDjson = current_path / "SJArchipelago_Tables - SID_to_APworldID.json"
    HRN_to_dictpickle = current_path / "SJArchipelago_Tables - Locations.pickle"


    with open(SID_to_APworldIDjson, 'w') as file:
        json.dump(sid_to_apworldid, file, indent=3)

    with open(HRN_to_dictpickle, 'wb') as file:
        pickle.dump(hrn_to_dict, file)
    
if __name__ == '__main__':
    main()