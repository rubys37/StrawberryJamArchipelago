import csv
import json
import pathlib

def main():
    current_path = pathlib.Path(__file__).parent.resolve()
    csv_file = current_path / "SJArchipelago_Tables - Locations Sheet.csv"
    with open(csv_file) as file:
        reader = csv.DictReader(file)
        data = [i for i in reader]
    apworldid_to_sid = {int(i["APWorld Location ID"]): i["Entity ID"] for i in data}
    sid_to_apworldid = {i["Entity ID"]: int(i["APWorld Location ID"]) for i in data}
    sid_to_hrn = {i["Entity ID"]: i["Human Readable Name"] for i in data}

    APworldIDjson = current_path / "SJArchipelago_Tables - APworldID_to_SID.json"
    SID_to_APworldIDjson = current_path / "SJArchipelago_Tables - SID_to_APworldID.json"
    SID_to_HRNjson = current_path / "SJArchipelago_Tables - SID_to_HRN.json"

    for i,j in zip([apworldid_to_sid, sid_to_apworldid, sid_to_hrn], [APworldIDjson, SID_to_APworldIDjson, SID_to_HRNjson]):
        with open(j, 'w') as file:
            file.write(json.dumps(i, indent=3))

    
if __name__ == '__main__':
    main()