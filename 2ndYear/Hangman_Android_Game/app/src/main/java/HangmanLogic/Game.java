package HangmanLogic;

import java.io.Serializable;

import hangman.linked_data_structures.SinglyLinkedList;

public class Game implements Serializable {
	private SinglyLinkedList<Character> wordToGuess;
	private SinglyLinkedList<Character> guessedLetters;
	private int mistakes;
	private static final int MAX_MISTAKES = 6;
	private boolean hintUsed = false;
	private int currentImageIndex;

	public Game() {

		this("Unknown");

	} // Game()

	public Game(String word) {
		this.wordToGuess = new SinglyLinkedList<>();
		this.guessedLetters = new SinglyLinkedList<>();
		this.mistakes = 0;
		for (char c : word.toCharArray()) {
			wordToGuess.add(c);
		}
	} // Game(String)

	public String getCurrentWord() {
		StringBuilder currentWord = new StringBuilder();
		for (int i = 0; i < wordToGuess.getLength(); i++) {
			currentWord.append(wordToGuess.getElementAt(i));
		}
		return currentWord.toString();
	} // getCurrentWord()

	// Getter and Setter for currentImageIndex
	public int getCurrentImageIndex() {
		return currentImageIndex;
	} // getCurrentImageIndex()

	public void setCurrentImageIndex(int currentImageIndex) {
		this.currentImageIndex = currentImageIndex;
	} // setCurrentImageIndex

	public int getMistakes() {
		return mistakes;
	}

	public String getWrongGuesses() {
		StringBuilder wrongGuesses = new StringBuilder();
		for (int i = 0; i < guessedLetters.getLength(); i++) {
			char guessedLetter = guessedLetters.getElementAt(i);
			if (!isLetterInWord(guessedLetter)) {
				wrongGuesses.append(guessedLetter).append(", ");
			}
		} // for
			// Remove the trailing comma and space if there are any wrong guesses
		if (wrongGuesses.length() > 0) {
			wrongGuesses = new StringBuilder(wrongGuesses.substring(0, wrongGuesses.length() - 2));
		}
		return wrongGuesses.toString();
	} // getWrongGuesses()

	public boolean isLetterInWord(char letter) {
		char lowerCaseLetter = Character.toLowerCase(letter);
		for (int i = 0; i < wordToGuess.getLength(); i++) {
			if (Character.toLowerCase(wordToGuess.getElementAt(i)) == lowerCaseLetter) {
				return true;
			}
		}
		return false;
	} // isLetterInWord(char)

	public int guessLetter(char letter) {
		letter = Character.toLowerCase(letter);

		if (isLetterAlreadyGuessed(letter)) {
			return -1; // Indicates the letter has already been guessed
		}

		guessedLetters.add(letter);

		if (isLetterInWord(letter)) {
			if (areAllLettersGuessed()) {
				return 1; // Indicates the player won the game
			} else {
				return 2; // Indicates a correct guess but the game is still in progress
			}
		} else {
			mistakes++;
			if (mistakes >= MAX_MISTAKES) {
				return 0; // Indicates the player lost the game
			}
			return 3; // Indicates an incorrect guess but the game is still in progress
		}
	} // guessLetter(char)

	public enum GuessResult {
		ALREADY_GUESSED, CORRECT_GUESS, INCORRECT_GUESS, WON, LOST
	} // GuessResult

	public GuessResult handleGuess(char guessedLetter) {
		if (isLetterAlreadyGuessed(guessedLetter)) {
			return GuessResult.ALREADY_GUESSED;
		}

		guessedLetters.add(guessedLetter);

		if (isLetterInWord(guessedLetter) || isSpecialCharacter(guessedLetter)) {
			if (areAllLettersGuessed()) {
				return GuessResult.WON;
			}
			return GuessResult.CORRECT_GUESS;
		} else {
			mistakes++;
			if (mistakes >= MAX_MISTAKES) {
				return GuessResult.LOST;
			}
			return GuessResult.INCORRECT_GUESS;
		}
	}

	public boolean areAllLettersGuessed() {
		for (int i = 0; i < wordToGuess.getLength(); i++) {
			char currentChar = wordToGuess.getElementAt(i);

			// Skip spaces and special characters
			if (currentChar == ' ' || !Character.isLetter(currentChar)) {
				continue;
			}

			if (!isLetterAlreadyGuessed(currentChar)) {
				return false;
			}
		}
		return true;
	} // areAllLettersGuessed()

	public boolean isLetterAlreadyGuessed(char guessedLetter) {
		for (int i = 0; i < guessedLetters.getLength(); i++) {
			if (Character.toLowerCase(guessedLetters.getElementAt(i)) == Character.toLowerCase(guessedLetter)) {
				System.out.println("Letter " + guessedLetter + " has already been guessed.");
				return true;
			}
		} // for
		return false;
	} // isLetterAlreadyGuessed(char)

	public SinglyLinkedList<Character> getGuessedLetters() {
		return guessedLetters;
	} // getGuessedLetters

	public SinglyLinkedList<Character> getWordToGuess() {
		return wordToGuess;
	} // getWordToGuess()

	public boolean hasGuessedLetter(char letter) {
		letter = Character.toLowerCase(letter);
		for (int i = 0; i < guessedLetters.getLength(); i++) {
			if (Character.toLowerCase(guessedLetters.getElementAt(i)) == letter) {
				return true;
			} // for
		}
		return false;
	} // hasGuessedLetter()

	public String getCurrentDisplay() {
		StringBuilder display = new StringBuilder();
		for (int i = 0; i < wordToGuess.getLength(); i++) {
			char c = wordToGuess.getElementAt(i);
			if (hasGuessedLetter(c) || !Character.isLetter(c) || isSpecialCharacter(c)) {
				display.append(c).append(" ");
			} else {
				display.append("_ ");
			}
		}
		return display.toString().trim();
	}

	private boolean isSpecialCharacter(char c) {
		// Define what you consider a 'special character' here
		// For simplicity, we'll consider non-ASCII characters as special
		return c > 127;
	}

	public boolean isOnlyOneLetterLeft() {
		int unguessedCount = 0;

		for (int i = 0; i < wordToGuess.getLength(); i++) {
			char currentChar = wordToGuess.getElementAt(i);
			if (Character.isLetter(currentChar) && !hasGuessedLetter(currentChar)) {
				unguessedCount++;
			}
		}

		return unguessedCount == 1;
	}

	public char getHint() {
		if (isOnlyOneLetterLeft()) {
			throw new IllegalStateException("Hint not allowed when only one letter is left.");
		}

		int wordLength = wordToGuess.getLength();
		int randomIndex;
		char hintLetter;

		do {
			randomIndex = (int) (Math.random() * wordLength);
			hintLetter = wordToGuess.getElementAt(randomIndex);
		} while (hasGuessedLetter(hintLetter) || !Character.isLetter(hintLetter));

		guessedLetters.add(hintLetter); // Mark the hint letter as guessed
		mistakes++; // Increment mistakes for using a hint
		return hintLetter;
	}

	public boolean isHintUsed() {
		return hintUsed;
	} // isHintUsed

	// Method to update image index based on the number of mistakes
	public void updateImageIndex() {
		// Assuming each mistake corresponds to a different image
		// and the HANGMAN_IMAGES array in your GameScreen class has a length of 7
		// (starting from 0 mistakes to 6 mistakes)
		this.currentImageIndex = this.mistakes;

		// To ensure the index does not exceed the array length
		if (this.currentImageIndex >= MAX_MISTAKES) {
			this.currentImageIndex = MAX_MISTAKES - 1;
		}
	}

}
