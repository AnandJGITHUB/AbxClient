# ABX Exchange Client

A simple C# client that connects to a custom ABX Exchange server, receives trade packets, identifies any missing sequences, requests them, and saves all packets in a formatted JSON file.

## Features

- Connects to a TCP server using IP and port.
- Requests a stream of trading packets.
- Detects missing packet sequences.
- Requests missing packets individually.
- Saves received and completed packet list as `packets.json`.

## Project Structure
. ├── Program.cs # Entry point of the application 
  ├── AbxClient.cs # Core logic for communication, parsing, and saving
  ├── Packet.cs # Represents a trade packet
  ├── Utils.cs # Utility functions (e.g., Big Endian parser)

## How It Works

1. Sends a request to stream all packets using command `0x01 0x00`.
2. Parses each 17-byte packet (Symbol, Side, Quantity, Price, Sequence).
3. Identifies and requests any missing sequences using command `0x02 <sequence>`.
4. Outputs all packets in order to `packets.json`.

## Packet Format (17 bytes)

| Field     | Offset | Length | Description        |
|----------|--------|--------|--------------------|
| Symbol   | 0      | 4      | ASCII characters   |
| Side     | 4      | 1      | ASCII character    |
| Quantity | 5      | 4      | Big endian int     |
| Price    | 9      | 4      | Big endian int     |
| Sequence | 13     | 4      | Big endian int     |

## Requirements

- .NET 6.0 or newer
- ABX Exchange server running on a known IP and port

## Usage

1. Clone or download the project.
2. Open in Visual Studio or any .NET-compatible IDE.
3. Modify `Program.cs` with the correct host and port:
   ```csharp
   var client = new AbxClient("127.0.0.1", 3000);

