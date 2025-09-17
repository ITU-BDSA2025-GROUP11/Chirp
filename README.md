How to use our program:

Our program is run by doing two tasks:
Task 1 - Run the web API:

1) In the terminal, enter the following directory:
   '../Chirp/CsvDbService'

2) now run the following command:
   dotnet run

Now you should see the following text:
"Now listening on: http://localhost:5000"

The web API is now running.

Task 2 - Using program

Now that the web API is running we can either write or print cheeps. This is done doing the following steps:

1) In a new terminal (keep your old terminal running with the web API) enter the following directory:
   '../Chirp/Chirp.CLI'
2) from here you decide if you want to write a new chirp or print the existing ones (see 3a for writing a new chirp, 3b for printing existing chirps)

3a) to write a chirp, run the following command:
dotnet run -- chirp "<your message>"

3b) to print the existing chirps, run the following command:
dotnet run -- print

  
