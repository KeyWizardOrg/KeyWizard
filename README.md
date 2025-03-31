# Microsoft Key Wizard 

Key Wizard is a tool that helps users discover and use keyboard shortcuts across different applications. It allows users to search for shortcuts using voice or text input, recognizing fuzzy searches and synonyms. Key Wizard can show searched up shortcuts, explain their purpose, and perform the action for you. It is a Windows-only application, meant to make everyday tasks simpler and more efficient.

## Installation

Key Wizard is available [here](https://apps.microsoft.com/detail/9nf4pjffzzms?hl=en-us&gl=US) on the Microsoft Store

The Key Wizard app runs in the background, and once installed, can be accessed using the keyboard shortcut **Ctrl + Alt + K**

## Extending the Code Using JSON
Key Wizard is an open source project that has the capacity for users adapt the app for any shortcuts they desire. The steps to personalize the code to add shortcuts are as follows:

Using the instructions on formatting the shortcuts, create a correctly formatted .json file and navigate to the Documents\Key Wizard folder. Once completed, drag and drop the correct file into the folder, and Key Wizard should automatically create a new folder for the shortcuts upon running. This is all that is needed for implementing new shortcuts.

Once this is complete, users will have successfully personalized their own Key Wizard tool.

## Formatting the Shortcuts

Name the file how you would like to group the shortcuts. For example, the .json file containing command prompt shorcuts is named CommandPrompt.json. For the instructions, the category Example will be used.

```bash
Example.json
```

Shortcuts are implemented in the following format
```json
{
    "Name": "Example",
    "Shortcuts": [
        {
            "Description": "TESTING 123",
            "Keys": [ "CTRL", "PLUS", "A", "Q" ]
        },
        {
            "Description": "ANOTHER TEST",
            "Keys": [ "CTRL", "PLUS", "B", "K"]
        }
    ]
}
```
In this example, the shortcut description may be **TESTING 123**, and the keyboard presses associated would be **CTRL + PLUS + A + Q**. 

For a classic keyboard shortcut example, this is the format that would be used for **Ctrl + C** (Copy)
```json       
 {
      "Description": "COPY",
      "Keys": [ "CTRL", "C"]
 }
```
By extending and personalizing the code, users are able to support almost any keyboard shortcut function.

When compiling the .json document, it is important that the key names match the syntax of those within the shortcuts/[Keys.cs](https://gitlab.scss.tcd.ie/sweng-25-group-12/sweng25_group12-microsoftkeywizard/-/blob/main/shortcuts/Keys.cs?ref_type=heads) file. 

For example, if the following appears in the Keys.cs file
```c#
public const byte OEM_COMMA = 0xBC;      // ,
```
The comma (,) button must be referenced as OEM_COMMA when used in Example.json

```c#
public const byte LEFT = 0x25;
```
The left arrow button can be referenced as LEFT 
```c#
public const byte A = 0x41;
```

Whereas the character A can simply be referenced as A

## Contributing

Key Wizard was designed in a way to encourage users to adapt based on their shortcut needs. The base of the Key Wizard has commands for PowerPoint, Command Prompt, File Explorer, and Text Editing. As the app already contains the keyboard press constants, has most basic keyboard shortcuts already available, and is implemented in a readable and well structured manor, it is an accessible app to customize. 

## About Us
Key Wizard was created by a team of 7 computer science students from Trinity College Dublin, with 2 professional mentors from Microsoft Dublin. This work has taken place as part of Trinity's Software Engineering Project Programme (SwEng), where students are paired with world-class mentors to work on a real project. 
