# Stock-analyzer### - README

Welcome to stock analyzer, a Windows Forms application developed for COP 4365. This project focuses on stock analysis through various recognizer algorithms. Below you'll find an overview of the project structure, how to set it up, and instructions for use.

---

## Table of Contents

- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Project Structure

The project consists of several key files and directories:

```
SSD Project 4
├── .vs/
├── WindowsFormsApp COP 4365.sln
└── WindowsFormsApp COP 4365/
    ├── App.config
    ├── Candlestick.cs
    ├── Form_StockViewer.cs
    ├── Form_StockViewer.Designer.cs
    ├── Form_StockViewer.resx
    ├── Program.cs
    ├── Recognizer.cs
    ├── Recognizer_Bearish.cs
    ├── Recognizer_Bearish_Engulfing.cs
    ├── Recognizer_Bearish_Harami.cs
    ├── Recognizer_Bullish.cs
    ├── Recognizer_Bullish_Engulfing.cs
    ├── Recognizer_Bullish_Harami.cs
    ├── Recognizer_Doji.cs
    ├── Properties/
    └── obj/
```

### Key Files

- **WindowsFormsApp COP 4365.sln**: Solution file for the project.
- **App.config**: Configuration file for the application.
- **Candlestick.cs**: Contains the Candlestick class.
- **Form_StockViewer.cs**: Main form for viewing stock data.
- **Recognizer.cs**: Base class for recognizer algorithms.
- **Recognizer_Bearish.cs**: Recognizer for bearish patterns.
- **Recognizer_Bullish.cs**: Recognizer for bullish patterns.
- **Properties/**: Contains application properties and resources.

## Prerequisites

- **Visual Studio 2019 or later**: This project is developed using Visual Studio.
- **.NET Framework 4.8**: Ensure you have the .NET Framework 4.8 installed on your system.

## Installation

1. **Clone the repository**:
    ```bash
    git clone https://github.com/yourusername/SSD-Project-4.git
    ```

2. **Open the solution**:
    - Navigate to the project directory.
    - Open `WindowsFormsApp COP 4365.sln` in Visual Studio.

3. **Restore NuGet packages**:
    - In Visual Studio, go to `Tools > NuGet Package Manager > Package Manager Console`.
    - Run the following command to restore the necessary packages:
      ```powershell
      Update-Package -reinstall
      ```

## Usage

1. **Build the solution**:
    - Press `Ctrl+Shift+B` to build the solution.

2. **Run the application**:
    - Press `F5` to start the application.

3. **Using the Stock Viewer**:
    - The main form, `Form_StockViewer`, will open.
    - Use the provided options to load and analyze stock data.
    - Recognizer algorithms will help identify bullish and bearish patterns.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add some feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Open a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

Thank you for using analyzer! If you have any questions or issues, feel free to open an issue in the repository.
