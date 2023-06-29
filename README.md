# InCoachAuswerter

This WPF-project is the result of a about 15 hours work for the "Zentrum für Positive Psychologie" in Lohne, Germany.
Goal is to provide a simple windows based WPF evaluation tool for emotion and feeling values of patients. To make this code work you need .NET-Framework 4.8 or higher and an IDE for C#.


## Inclusions/Packages

The following packages and frameworks are included:
- LiveCharts (0.9.7)
- ModernWpfUI (0.9.7-preview.2)
- Newtonsoft.Json (13.0.3)


## How does it work

The functionality of the whole app can be broken down to five steps:

```mermaid
graph LR
A[Start application]  --> B[Load patient data] --> C[Validate patient data] --> D[Import patient data] --> E[Evaluate patient data]

```

### Start application

Simple and easy: You start the application and load MainWindow which navigates you to the Loading Page automatically.

### Load patient data

You load txt-based patient data by selecting them from your hard drive and display the data inside a ContentDialog.

### Validate patient data

By clicking "Validate" inside the ContentDialog you execute the PatientFileValidateService. This service transforms the data string into json format. If this is successful you'll be able to import your data.

### Import patient data

By clicking "Import" inside the ContentDialog you execute the PatientFileReaderService. the patient data string will be converted to json and written inside the "PatientData.json". You'll get to the OverviewPage to evaluate the data.


### Evaluate patient data

This dialog gives you the opportunity to visualize the patient data inside a simple graph. By switching the graphs in the upper right corner you'll be able to see the lines in single view.


## Copyright

Feel free to do whatever you want with the source code to make yourself or others happy. As long as you don't harm anybody or use the pictures (copyright: Zentrum für Positive Psychologie Lohne, Germany) inside your projects you are free to express yourself.
