package sudokugame;

import java.io.BufferedReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStreamReader;

/**
 * Title:       SudokuInterface
 * Description: This is the Frontend of the Sudoku Game. 
 * 							The undo function is not in here because I couldn't make it work in time. 
 * 							(Will make it work in Frame the part though).
 * 3 31, 2023  
 */

public class SudokuInterface {

	SudokuGame game1 = new SudokuGame();
	private String fileName;

	public SudokuInterface() {
		String fileName = getFileName();
		try {
			game1 = new SudokuGame(fileName);
		}
		catch (IOException e) {

			System.err
					.println("The file does not exist!! Start the game and try again!");
			System.exit(-1);

		}

	} // getting the fileName.

	//Getting the filename.
	public String getFileName() {
		BufferedReader reader = new BufferedReader(
				new InputStreamReader(System.in));
		fileName = "";
		System.out
				.print("Enter file name (or press enter for default sudoku.txt): ");
		try {
			fileName = reader.readLine();
		}
		catch (IOException e) {
			System.out.println("Error reading input.");
		}

		if (fileName.equals("")) {
			fileName = "sudoku.txt";
		}

		return fileName;
	} // getFileName()

	// To save on command of "s".
	public void save(String filename, int[][] board) {
		try {
			FileWriter writer = new FileWriter(filename);
			String delimiter = "~";

			for (int row = 0; row < 9; row++) {
				for (int col = 0; col < 9; col++) {
					int value = board[row][col];
					writer.write(value == 0 ? "*" : Integer.toString(value));
					writer.write(delimiter);
				}
				writer.write("\n");
			}

			writer.close();

			System.out.println("Board saved to " + filename + "."
					+ " Thanks for playing! Come Again!");
		}
		catch (IOException e) {
			System.err.println("Error saving board to file: " + e.getMessage());
		}
		System.exit(-1);
	} // save(String, int board[][])

	// To print the board on screen.
	public void printBoard() {
		int[][] board = game1.getBoard();
		System.out.println("\nYour board: ");
		for (int row = 0; row < 9; row++) {
			for (int col = 0; col < 9; col++) {
				if (board[row][col] == 0) {
					System.out.print("* ");
				}
				else {
					System.out.print(board[row][col] + " ");
				}

				if ((col + 1) % 3 == 0 && col != 8) {
					System.out.print("| ");
				}
			}

			System.out.println();

			if ((row + 1) % 3 == 0 && row != 8) {
				System.out.println("---------------------");
			}
		}
	} // printBoard()

	public void play() {
		BufferedReader reader = new BufferedReader(
				new InputStreamReader(System.in));
		int[][] board = game1.getBoard();
		System.out
				.println("Type Q at any time to exit the game & S to save the game");
		while (!game1.isComplete(board)) {
			System.out
					.println("Enter row (1-9) and column (1-9) separated by a space.");
			String input;
			try {
				input = reader.readLine();
			}
			catch (IOException e) {
				System.err.println("Error reading input.");
				input = "";
			}
			if (input.equalsIgnoreCase("q")) {
				System.exit(-1);
			}
			else
				if (input.equalsIgnoreCase("s")) {
					save(getFileName(), board);
				}
				else
					if (input.equalsIgnoreCase("u")) {

					}
			String[] values = input.split(" ");
			if (values.length != 2) {
				System.err.println(
						"Invalid input. Please give 2 integer numbers between (1-9)!");
			}
			else {
				int row = Integer.parseInt(values[0]) - 1;
				int col = Integer.parseInt(values[1]) - 1;

				if (row < 0 || row > 8 || col < 0 || col > 8) {
					System.err.println("Please enter values from (1-9).");
				}
				else
					if (board[row][col] != 0) {
						System.err.println("That square is already filled.");
					}
					else {

						System.out.print("Enter value (1-9) to put in square: ");

						int value;
						try {
							value = Integer.parseInt(reader.readLine());
						}
						catch (NumberFormatException | IOException e) {
							System.err.println("Invalid input.");
							value = 0;
						}

						if (value < 1 || value > 9) {
							System.err.println("Please enter values from (1-9)");
						}
						else
							if (game1.isValueInRow(board, value, row)) {
								System.err.println("Value is already in the row.");
							}
							else
								if (game1.isValueInCol(board, value, col)) {
									System.err.println("Value is already in the col.");
								}
								else
									if (game1.isValueInThreeGrid(board, value, col, row)) {
										System.err.println("Value is already in the 3x3 grid.");
									}
									else {
										board[row][col] = value;
										printBoard();
									}
						if (game1.isComplete(board)) {
							System.out
									.println("\nWow! You are a Master! Thanks for Playing!");
							System.exit(-1);
						}
					}
			}
		}
	} // play()

	public static void main(String[] args) {
		SudokuInterface ui = new SudokuInterface();
		ui.printBoard();
		ui.play();
	} // main()
} // SudokuInterface
