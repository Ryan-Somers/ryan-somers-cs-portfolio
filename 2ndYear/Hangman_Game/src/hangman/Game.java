package hangman;

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
		String currentWord = "";
		for (int i = 0; i < wordToGuess.getLength(); i++) {
			currentWord += wordToGuess.getElementAt(i);
		}
		return currentWord;
	} // getCurrentWord()

	// Getter and Setter for currentImageIndex
	public int getCurrentImageIndex() {
		return currentImageIndex;
	} // getCurrentImageIndex()

	public void setCurrentImageIndex(int currentImageIndex) {
		this.currentImageIndex = currentImageIndex;
	} // setCurrentImageIndex

	public String getWrongGuesses() {
		String wrongGuesses = "";
		for (int i = 0; i < guessedLetters.getLength(); i++) {
			char guessedLetter = guessedLetters.getElementAt(i);
			if (!isLetterInWord(guessedLetter)) {
				wrongGuesses += guessedLetter + ", ";
			}
		} // for
			// Remove the trailing comma and space if there are any wrong guesses
		if (wrongGuesses.length() > 0) {
			wrongGuesses = wrongGuesses.substring(0, wrongGuesses.length() - 2);
		}
		return wrongGuesses;
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

		if (isLetterInWord(guessedLetter)) {
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
	} // GuessResult HandleGuess(char)

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
		String display = "";
		for (int i = 0; i < wordToGuess.getLength(); i++) {
			char c = wordToGuess.getElementAt(i);
			boolean isGuessed = hasGuessedLetter(c);

			if (isGuessed || c == ' ' || !Character.isLetter(c)) {

				display += c + " ";
			} else {
				display += "_ ";
			}
		}
		return display.trim();
	} // getCurrentDisplay()

	public boolean isOnlyOneLetterLeft() {
		int unguessedCount = 0;

		for (int i = 0; i < wordToGuess.getLength(); i++) {
			if (!hasGuessedLetter(wordToGuess.getElementAt(i))) {
				unguessedCount++;
			}
		} // for

		return unguessedCount == 1;
	} // isOnlyOneLetterLeft()

	public char getHint() {
		int wordLength = wordToGuess.getLength();
		int randomIndex;
		char hintLetter;

		// This loop will keep running until we find an unguessed letter
		do {
			randomIndex = (int) (Math.random() * wordLength);
			hintLetter = wordToGuess.getElementAt(randomIndex);
		} while (hasGuessedLetter(hintLetter) || !Character.isLetter(hintLetter)); // Ensure the letter hasn't been
																					// guessed and is a valid letter

		hintUsed = true;
		guessedLetters.add(hintLetter); // Mark the hint letter as guessed

		return hintLetter;
	} // getHint()

	public boolean isHintUsed() {
		return hintUsed;
	} // isHintUsed

}
