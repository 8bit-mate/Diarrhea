# Diarrhea

**D**aria's **I**nferno **A**udio **R**esources **R**epacking/**H**acking/**E**xtracting **A**pplication.

A command-line tool to work with the Hypnotix’es [Daria's Inferno](https://en.wikipedia.org/wiki/Daria%27s_Inferno) in-game audio.

The game utilizes a simple binary container that contains all in-game audio. It located at the `{Game Directory}\audioLib\Audio1.dat` binary file. Audio files are packed with their original file names (but the file extension is not included). The game loads a certain audio file using the file name as an identifier. Each audio file is a WAVE file.

The tool is compatible with the other games developed by the Hypnotix (e.g. the *Deer Avenger 4* and the *Who Wants to Beat Up a Millionaire*), although some adjustments might be required (e.g. the binary container from the *Deer Avenger 4* game does include audio file extensions in the file names).

# Installation

No installation required. Just download the latest release and unpack files to a directory.

# Usage

General syntax is:

    > Diarrhea.exe <command> [options]

Use the `--help` flag to get the list of available commands:

    > Diarrhea.exe --help

Call a command with the `--help` flag to get help information about the command:

    > Diarrhea.exe <command> --help

## Main commands

  * `extract`:        Extract individual files.

  * `extract-all`:    Extract all files.

  * `list`:           List files on a *.dat container.

  * `packdir`:        Pack a directory with files into a *.dat container.

## Usage example to replace in-game audio

1.	Extract original in-game audio files:

        > Diarrhea.exe extract-all -i d:\Audio1_original.dat -p orig_ -s .wav -o d:\wav_files

    The `-p orig_` option will add a prefix to all file names, while the `-s .wav` option will add a suffix (.wav file extension) to all file names.

2.	Replace some files with your own versions. You might want to mark those files with the `edited_` prefix to distinguish them from the unedited files.

3.	Pack the directory back into a container:

        > Diarrhea.exe packdir -i d:\ wav_files -o d:\Audio1.dat --regex="(?<=_).*(?=.wav$)"

    The regular expression will remove all prefixes (marked with the underscore character) and suffixes (file extensions) from the file names.

4.	Ensure that files are packed:

        > Diarrhea.exe list -i d:\Audio1.dat

5.	Replace `{Game Directory}\audioLib\Audio1.dat` file with the generated `Audio1.dat` file. Now the game should play your custom audio.
