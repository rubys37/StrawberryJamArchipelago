import csv
import pathlib

def main():
    csv_paths = pathlib.Path.cwd() / "entity_id_sheets"
    csv_data = []
    for csv_path in csv_paths.iterdir():
        with open(csv_path) as file:
            csv_data.extend([f'{{"{i[0]}", "{i[2]}"}},\n' for i in csv.reader(file) if i])
    with open('entity_dict.txt', 'w') as file:
        file.writelines(csv_data)

if __name__ ==  '__main__':
    main()