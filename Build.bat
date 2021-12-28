@echo off

quicktype -o Model/BackHrModel.cs -l cs -s schema --src "schema.json"^
            --namespace HR_Management.Model --csharp-version 5 --array-type list^
            --number-type double --features just-types --base-class Object
