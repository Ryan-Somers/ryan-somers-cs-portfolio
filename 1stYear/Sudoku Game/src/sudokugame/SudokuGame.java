package sudokugame;

import java.io.*;
import java.util.Scanner;

import javax.swing.JTextField;

/**
 * Title:       SudokuGame
 * Description: This is the backend of the SudokuGame.
 *              Methods to verify are in here as well. 
 * 3 31, 2023  
 */

public class SudokuGame {

	private static int[][] board = new int[9][9];
	public static final int GRID_SIZE = 9;

	public SudokuGame() {

	}

	// Reading in the board from the desired filename.
	public SudokuGame(String fileName) throws IOException {
		board = new int[9][9];

		BufferedReader reader = new BufferedReader(new FileReader(fileName));
		String line;

		for (int row = 0; row < 9; row++) {
			line = reader.readLine();
			String[] values = line.split("~");

			for (int col = 0; col < 9; col++) {
				if (values[col].equals("*")) {
					board[row][col] = 0;
				}
				else {
					board[row][col] = Integer.parseInt(values[col]);
				}
			}
		} // SudokuGame(String fileName)

		reader.close();
	} // SudokuGame(String)

	public boolean isValidFileFormat(File file) {
		try (Scanner scanner = new Scanner(file)) {
			while (scanner.hasNextLine()) {
				String line = scanner.nextLine();
				String[] tokens = line.split("~");
				if (tokens.length != 9) {
					return false; // line does not have 9 tokens
				}
				for (String token : tokens) {
					if (!token.equals("*") && !Character.isDigit(token.charAt(0))) {
						return false; // token is not an asterisk or a digit
					}
				}
			}
			return true; // all lines are valid
		}
		catch (FileNotFoundException e) {
			return false; // file not found
		}
	}

	public int[][] getBoard() {
		return board;
	} // getBoard()

	public boolean isValid(int[][] board, int num, int col, int row) {
		if (isValueInCol(board, num, col) || isValueInRow(board, num, row)
				|| isValueInThreeGrid(board, num, col, row)) {
			return true;
		}
		return false;
	} // isValid(int, int, int, int)

	public boolean isValueInThreeGrid(int[][] board, int num, int col, int row) {
		// Calculate the starting row and column indices of the box
		int boxStartRow = row - (row % 3);
		int boxStartCol = col - (col % 3);

		// Check if the value is already present in the box
		for (int r = boxStartRow; r < boxStartRow + 3; r++) {
			for (int c = boxStartCol; c < boxStartCol + 3; c++) {
				if (board[r][c] == num) {
					return true;
				}
			}
		}

		// Value is not in the box
		return false;
	} // isValueInThreeGrid(int [][]board, int, int, int)

	public boolean isValueInRow(int[][] board, int value, int row) {
		for (int i = 0; i < GRID_SIZE; ++i) {
			if (board[row][i] == value) {
				return true;
			}
		}
		return false;
	} // isValueInRow(int board[][], int, int)

	public boolean isValueInCol(int[][] board, int num, int col) {
		for (int i = 0; i < GRID_SIZE; ++i) {
			if (board[i][col] == num) {
				return true;
			}
		}
		return false;
	} // isValueInCol(int board[][], int, int)

	public boolean isValueInBox(int[][] board, int col, int row) {
		for (row = 0; row < GRID_SIZE; ++row) {
			for (col = 0; col < GRID_SIZE; ++col) {
				if (board[row][col] != 0) {
					return true;
				}
			}
		}
		return false;
	} // isValueInBox(int[][] board, int, int)

	public boolean isComplete(int[][] board) {
		for (int row = 0; row < 9; row++) {
			for (int col = 0; col < 9; col++) {
				if (board[row][col] == 0) {
					return false;
				}
			}
		}
		return true;
	} // isComplete(int board[][])

	public boolean isFrameComplete(JTextField grid[][]) {
		boolean allFieldsFilled = true;
		for (int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				if (grid[i][j].isEnabled()) {
					allFieldsFilled = false;
					break;
				}
			}
		}
		return allFieldsFilled;
	} // isFrameComplete(JTextField)

	public boolean isOutOfBounds(int[][] board, int row, int col) {
		if (board[row][col] > 9 || board[row][col] < 1) {
			return true;
		}
		else {
			return false;
		}
	} // isOutOfBounds(int [][], int, int)
	
	public boolean isInSubGrid(int[][] board, int value, int row, int col) {
    int rowOffset = (row / 3) * 3;
    int colOffset = (col / 3) * 3;

    for (int i = rowOffset; i < rowOffset + 3; i++) {
        for (int j = colOffset; j < colOffset + 3; j++) {
            if (board[i][j] == value) {
                return true;
            }
        }
    }
    return false;
}
	
	public void setBoard(int[][] newBoard) {
    board = newBoard;
}


} // SudokuGame
