package hangman;

import static org.junit.jupiter.api.Assertions.*;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import hangman.linked_data_structures.SinglyLinkedList;

class GameTest {

	private Game game;

	@BeforeEach
	public void setUp() {
		game = new Game("example");
	}

	@Test
	void testCorrectGuess() {
		assertEquals(Game.GuessResult.CORRECT_GUESS, game.handleGuess('e'));
	}

	@Test
	void testIncorrectGuess() {
		assertEquals(Game.GuessResult.INCORRECT_GUESS, game.handleGuess('z'));
	}

	@Test
	void testAlreadyGuessedLetter() {
		game.handleGuess('e');
		assertEquals(Game.GuessResult.ALREADY_GUESSED, game.handleGuess('e'));
	}

	@Test
	void testDisplayAfterCorrectGuess() {
		game = new Game("example");
		game.handleGuess('e');
		assertEquals("e _ _ _ _ _ e", game.getCurrentDisplay());
	}

	@Test
	void testDisplayAfterIncorrectGuess() {
		game.handleGuess('z');
		assertEquals("_ _ _ _ _ _ _", game.getCurrentDisplay());
	}

	@Test
	void testHintUsage() {
		char hint = game.getHint();
		assertTrue(game.isLetterInWord(hint));
		assertTrue(game.isHintUsed());
	}

	@Test
	void testOneLetterLeft() {
		game.handleGuess('e');
		game.handleGuess('x');
		game.handleGuess('a');
		game.handleGuess('m');
		game.handleGuess('p');
		assertTrue(game.isOnlyOneLetterLeft());
	}

	@Test
	void testNoLetterLeftAfterWin() {
		game.handleGuess('e');
		game.handleGuess('x');
		game.handleGuess('a');
		game.handleGuess('m');
		game.handleGuess('p');
		game.handleGuess('l');
		assertFalse(game.isOnlyOneLetterLeft());
	}

	@Test
	void testWordToGuessList() {
		SinglyLinkedList<Character> wordList = game.getWordToGuess();
		assertEquals(7, wordList.getLength());
		assertEquals(Character.valueOf('e'), wordList.getElementAt(0));
		assertEquals(Character.valueOf('e'), wordList.getElementAt(6));
	}

	@Test
	void testGuessedLettersListAfterGuess() {
		game.handleGuess('e');
		SinglyLinkedList<Character> guessedList = game.getGuessedLetters();
		assertEquals(1, guessedList.getLength());
		assertEquals(Character.valueOf('e'), guessedList.getElementAt(0));
	}

	@Test
	void testGuessedLettersListMultipleGuesses() {
		game.handleGuess('e');
		game.handleGuess('x');
		SinglyLinkedList<Character> guessedList = game.getGuessedLetters();
		assertEquals(2, guessedList.getLength());
		assertEquals(Character.valueOf('e'), guessedList.getElementAt(0));
		assertEquals(Character.valueOf('x'), guessedList.getElementAt(1));
	}

	@Test
	void testIsLetterInWord() {
		assertTrue(game.isLetterInWord('e'));
		assertFalse(game.isLetterInWord('z'));
	}

	@Test
	void testIsLetterAlreadyGuessed() {
		assertFalse(game.isLetterAlreadyGuessed('e'));
		game.handleGuess('e');
		assertTrue(game.isLetterAlreadyGuessed('e'));
	}

	@Test
	void testHasGuessedLetter() {
		assertFalse(game.hasGuessedLetter('e'));
		game.handleGuess('e');
		assertTrue(game.hasGuessedLetter('e'));
	}

	@Test
	void testGetHint() {
		char hint = game.getHint();
		assertTrue(game.isLetterInWord(hint));
		assertTrue(game.hasGuessedLetter(hint));
	}

	@Test
	void testGetHintDoesNotRepeat() {
		char hint1 = game.getHint();
		char hint2 = game.getHint();
		assertNotEquals(hint1, hint2);
	}

	@Test
	void testAreAllLettersGuessed() {
		assertFalse(game.areAllLettersGuessed());
		game.handleGuess('e');
		game.handleGuess('x');
		game.handleGuess('a');
		game.handleGuess('m');
		game.handleGuess('p');
		game.handleGuess('l');
		assertTrue(game.areAllLettersGuessed());
	}
}
