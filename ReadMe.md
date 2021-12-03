# KommissarsRTWTools
###### Selection of useful (and developing) tools for modders old and new

Kommissars RTW Tools are designed in mind with my own work first and foremost, although I'm more than happy to take community requests. Idea behind them is automation of certain tasks that normally take up a long time, or are just straight up mind-numbingly boring.

## RegionExtractor
Extracts regions from "descr_regions.txt" file. Make sure you provide a full path, otherwise will result in errors. At the moment it does not look for any errors and assumes the file is formatted correctly, which, I mean, it should be anyway.
Results are saved into RegionNames and SettlementNames text files. You can copy and paste them into excel tables if you wish - they'll match.

## RomeScriptGenerator
At the moment uses whatever is inside of "SampleScript.txt" and "SettlementNames.txt". Keyword SETTLEMENT is going to be replaced with names of all settlements found in the file. Good for automating settlement scripts. Functionality will be expanded in the future.
Result is saved in "NewScript.txt"

## UIHelper
Conists of two aides: UnitMaker and BuildingMaker.

###BuildingMaker
Uses whatever is in "Data > BuildingMaker" folder. Anything with a keyword "CULTUREWORD" will get renamed to a culture tag, like "barbarian" or "carthage". Good for quickly creating building icons from one master. Results are saved in the output folder.

###UnitMaker
UnitMaker will go throug the provided CSV file (excel file provided as it contains nicer formatting, but can't be used for generation). It will create all UnitCards based on prefies, in the folder specified in "Setup.txt" file. Please have a look at the build for an example.