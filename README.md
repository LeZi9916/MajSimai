# MajSimai

MajSimai is an interpreter for [Simai](https://w.atwiki.jp/simai/), written in [C#](https://learn.microsoft.com/en-us/dotnet/csharp/).

## Features

### Metadata

- [x] Basic metadata fields
- [x] Comments `||Hello world!`
- [x] Custom simai commands with '=' as separator `&commandPrefix=value`

### Chart

- [x] BPM declaration `(float)`
- [x] Beats declaration
  - [x] Common `{int}`
  - [x] Absolute time `{#float}`
- [x] Tap `1`
  - [x] Break flag `1b`
  - [x] EX flag `1x`
  - [x] Mine flag `1m`
  - [x] Force star flag `1$`
    - [x] Fake rotation `1$$`
- [x] Hold `1h[duration]`
  - [x] Short form `1h`
  - [x] Break flag
    - `1bh`
    - `1hb`
  - [x] Mine flag
    - `1bm`
    - `1mb`
  - Duration format:
    - [x] Common `[int:int]`
    - [x] With absolute time `[#float]`
    - [x] With custom BPM `[float#int:int]`
- [x] Slide `x-y[duration]`
  - [x] Same head `1-3[duration]*-7[duration]`
  - [x] Break flag
    - `1-3[duration]b`
    - `1-3b[duration]`
  - [x] Mine flag
    - `1-3[duration]m`
    - `1-3m[duration]`
  - [x] Conn slide
    - `1-3-5-7[duration]`
    - `1-3[duration]-5[duration]-7[duration]`
  - [x] No head slide:
    - [x] `1?-3[duration]`
    - [x] `1!-3[duration]`
  - Duration format:
    - [x] `[160#2.0]`
    - [x] `[3.0##1.5]`
    - [x] `[3.0##4:1]`
    - [x] `[3.0##160#4:1]`
- [x] Touch `B1`
  - [x] Hanabi flag `B1f`
  - [x] Break flag `B1b`
  - [x] Mine flag `B1m`
  - [x] Ex flag `B1x`
- [x] Touch hold `B1h[duration]`
  - [x] Short form `B1h`
  - [x] Hanabi flag `B1hf[duration]`
  - [x] Break flag `B1hb[duration]`
  - [x] Mine flag `B1hm[duration]`
  - [x] Ex flag `B1hx[duration]`
  - Duration format:
    - [x] Common `[int:int]`
    - [x] With absolute time `[#float]`
    - [x] With custom BPM `[float#int:int]`
- [x] Each note `note/note`
  - [x] Fake each ``1`2`3`4,``
- [ ] EOF flag `E`

## Getting Started

To use MajSimai in your own project, you will need to add a reference to the MajSimai library in your solution.

```C#
// Specify the chart file
var filePath = "/path/to/your/chart.txt";
using var fileStream = File.OpenRead(filePath);

// Parse into SimaiFile
var simaiFile = await SimaiParser.ParseAsync(fileStream);
// With special encoding
simaiFile = await SimaiParser.ParseAsync(fileStream, Encoding.ASCII);

// Or parse only metadata
var simaiMetadata = await SimaiParser.ParseMetadataAsync(fileStream);
// With special encoding
simaiMetadata = await SimaiParser.ParseMetadataAsync(fileStream, Encoding.ASCII);


// Or parse from the chart text you provide
var fumen = "(197){8}1,1,1,1,1,1,1,1,2b,3h,4-1[8:1],";
var simaiChart = await SimaiParser.ParseChartAsync(fumen);

```

## Build

If you want to compile MajSimai yourself, please make sure that `dotnet` is installed on your computer.

The target frameworks available for MajSimai are as follows:

- `netstandard2.1`
- `net5.0`
- `net6.0`
- `net7.0`
- `net8.0`
- `net9.0`

Then, use the following method to compile it:

```bash
git clone https://github.com/LingFeng-bbben/MajSimai.git

cd MajSimai

dotnet restore

# Feel free to change the target framework
# If you want to generate nuget package, please remove /p:GeneratePackageOnBuild=false
dotnet publish -c release /p:PublishAot=false /p:GeneratePackageOnBuild=false -f netstandard2.1

```

MajSimai is AOT and Trim compatible.

If you want to compile native library, you need to specify the use of AOT.

```bash
dotnet publish -c release /p:PublishAot=true -f net9.0 -r linux-x64
```

In the native library, MajSimai will export functions:

- `MajSimai!MajSimai_Parse`
- `MajSimai!MajSimai_Free`
- `MajSimai!MajSimai_FreeHGlobal`

You can use these exported functions in C++:

```C++
struct MajSimaiParseResult
{
    int32_t code = -1;
    int32_t errorAtLine = -1;
    int32_t errorAtColumn = -1;
    int32_t errorMsgLen = 0;
    int32_t errorContentLen = 0;

    void* simaiFile = NULL;
    char* errorMsgAnsi = NULL;
    char* errorContentAnsi = NULL;
};

extern "C" void __cdecl MajSimai_Parse(char*, int32_t, void*);
extern "C" bool __cdecl MajSimai_Free(MajSimaiParseResult*);
extern "C" bool __cdecl MajSimai_FreeHGlobal(void*);

int main()
{
    const string FUMEN = R"(&title=Test
&artist=NULL
&first=0
&des=1
&lv_5=114514
&inote_5=
(197){8}1,2,3,4,5,6,7,8,)";
    MajSimaiParseResult result = MajSimaiParseResult();
    MajSimai_Parse(FUMEN.data(), FUMEN.length(), &result);
    MajSimai_Free(&result)
```
