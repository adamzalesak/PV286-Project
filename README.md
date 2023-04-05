# Panbyte

Panbyte is a converter of data between different formats. It is a console application written in C#. More
information can be seen by running the application with the `--help` argument.

# Prerequisites
To build and run this project, you'll need to have the following software installed on your system:
*   .NET 7.0 SDK

# Building
To build the project, run the following command in the root directory of the project:
```bash
dotnet build
```

The executable file will be located in the `./src/Panbyte.App/bin/Debug/net7.0` directory.

# Running
To run the project, run the following command in the root directory of the project:
```bash
dotnet run --project ./src/Panbyte.App -- [arguments]
```

# Testing
To run the unit and integration tests, run the following command in the root directory of the project:
```bash
dotnet test
```

## Authors
*   Filip Hájek, 493341
*   Katarína Platková, 493144
*   Adam Zálešák, 493071
