package com.example.hangmangame;

import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;


import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.util.TypedValue;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.PopupMenu;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.Toolbar;


import com.google.android.material.navigation.NavigationView;

import HangmanLogic.Dictionary;
import HangmanLogic.Game;
import HangmanLogic.GameStateManager;
import HangmanLogic.Player;
import HangmanLogic.Scoreboard;

public class GameScreen extends AppCompatActivity implements View.OnClickListener {
    private Game game; // Instance of my Game class
    private Dictionary dictionary;
    private ImageView hangmanImage;
    private TextView wordText;
    private TextView wrongGuessLetters;
    private Button hintBtn;
    private String currentPlayerName;
    private static final int MAX_MISTAKES = 6;
    private Scoreboard scoreboard;
    private  Player player;


    private static final int[] HANGMAN_IMAGES = {
            R.drawable.framestart, // Assuming this is for 0 mistakes
            R.drawable.frame1,     // For 1 mistake
            R.drawable.frame2,     // For 2 mistakes
            R.drawable.frame3,     // And so on...
            R.drawable.frame4,
            R.drawable.frame5,
            R.drawable.framegone   // For 6 mistakes
    };
    Button letterBtn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // Initialize UI components
        hangmanImage = findViewById(R.id.hangmanImage);
        wordText = findViewById(R.id.wordText);
        wrongGuessLetters = findViewById(R.id.wrongGuessLetters);
        LinearLayout row1 = findViewById(R.id.row1);
        LinearLayout row2 = findViewById(R.id.row2);
        LinearLayout row3 = findViewById(R.id.row3);
        hintBtn = findViewById(R.id.hintBtn);

        // Set up buttons and other UI elements
        String row1Keys = "QWERTYUIOP";
        String row2Keys = "ASDFGHJKL";
        String row3Keys = "ZXCVBNM";
        createButtonsForRow(row1, row1Keys);
        createButtonsForRow(row2, row2Keys);
        createButtonsForRow(row3, row3Keys);

        hintBtn.setOnClickListener(v -> useHint());

        scoreboard = GameStateManager.loadState(this);
        Intent intent = getIntent();
        currentPlayerName = intent.getStringExtra("playerName");

        ImageView hamburgerMenu = findViewById(R.id.hamburgerSVG);

        hamburgerMenu.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showPopupMenu(view);
            }
        });

        try {
            initializeGameForPlayer(currentPlayerName);

            // Update UI based on game state
            wordText.setText(game.getCurrentDisplay());
            wrongGuessLetters.setText(game.getWrongGuesses());
            updateHangmanImage();
        } catch (IllegalStateException e) {
            showNoWordsLeftDialog();
        }
        hintBtn = findViewById(R.id.hintBtn);
        hintBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                useHint();
            }
        });
    }

    private void initializeGameForPlayer(String playerName) {
        Player currentPlayer = scoreboard.getPlayerByName(playerName);

        if (currentPlayer != null && currentPlayer.getGameState() != null) {
            // Load existing game state
            game = currentPlayer.getGameState();
            updateHangmanImage(); // Update UI with the correct image
            player = currentPlayer; // Assign the existing player
        } else {
            // Player doesn't exist or has no game state, start a new game
            currentPlayer = new Player(playerName); // Create new player
            scoreboard.addPlayer(currentPlayer);

            try {
                dictionary = new Dictionary(this);
                String randomWord = dictionary.getRandomWord();
                game = new Game(randomWord);
                currentPlayer.setGameState(game); // Set the new game for the player
            } catch (IllegalStateException e) {
                showNoWordsLeftDialog(); // Call the method to show the dialog
                return; // Exit the method to prevent further execution
            }

            player = currentPlayer; // Assign the new player
        }
    }

    @Override
    public void onClick(View view) {
        int clickBtn = view.getId();
        if (view instanceof Button) {
            letterBtn = (Button) view;
            char guessedLetter = letterBtn.getText().charAt(0);
            Game.GuessResult result = game.handleGuess(guessedLetter);

            // Update hangman image here based on current mistakes
            updateHangmanImage();

            // Update the word display and wrong guesses
            wordText.setText(game.getCurrentDisplay());
            wrongGuessLetters.setText(game.getWrongGuesses());

            // Handle game win or loss
            if (result == Game.GuessResult.WON) {
                showEndGameDialog(true);
                player.incrementGamesWon();
                player.incrementGamesPlayed();
            } else if (result == Game.GuessResult.LOST) {
                showEndGameDialog(false);
                player.incrementGamesPlayed();
            }
            checkIfHintAllowed();
        }
        if (clickBtn == R.id.hintBtn) {
            useHint();
        }

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.drawer_menu, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();

        if (id == R.id.itemNewGame) {
            restartGame();
            return true;
        } else if (id == R.id.itemScoreBoard) {
            Intent i = new Intent(this, ScoreboardScreen.class);
            startActivity(i);
            return true;
        } else if (id == R.id.itemRules) {
            Intent i = new Intent(this, HelpScreen.class);
            startActivity(i);
            return true;
        } else if (id == R.id.itemExit) {
            finish();
            return true;
        } else {
            return super.onOptionsItemSelected(item);
        }
    }


    private void showPopupMenu(View view) {
        PopupMenu popupMenu = new PopupMenu(this, view);
        MenuInflater inflater = popupMenu.getMenuInflater();
        inflater.inflate(R.menu.drawer_menu, popupMenu.getMenu());

        popupMenu.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
            @Override
            public boolean onMenuItemClick(MenuItem item) {
                // Handle the menu item click here, or call onOptionsItemSelected(item)
                return onOptionsItemSelected(item);
            }
        });

        popupMenu.show();
    }


    private void createButtonsForRow(LinearLayout rowLayout, String keys) {
        for (int i = 0; i < keys.length(); i++) {
            Button btn = new Button(this);
            btn.setText(String.valueOf(keys.charAt(i)));
            btn.setOnClickListener(this);

            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.WRAP_CONTENT, // Adjust width as needed
                    LinearLayout.LayoutParams.WRAP_CONTENT  // Adjust height as needed
            );
            params.weight = 1.0f;
            int margin = (int) TypedValue.applyDimension(
                    TypedValue.COMPLEX_UNIT_DIP, 3, getResources().getDisplayMetrics()
            ); // 4dp margin, adjust as needed
            params.setMargins(margin, margin, margin, margin);
            btn.setLayoutParams(params);
            btn.setTextSize(TypedValue.COMPLEX_UNIT_SP, 20);

            btn.setBackground(ContextCompat.getDrawable(this, R.drawable.button_background));

            rowLayout.addView(btn);
        }
    }

    private void showEndGameDialog(boolean won) {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(won ? "You won!" : "You lost!");
        builder.setMessage(won ? "Congratulations! You guessed the word! The word was: " + game.getCurrentWord() + " \nWould you like to play again?" : "\nThe word was: " + game.getCurrentWord() + ". Would you like to play again?");

        builder.setPositiveButton("Yes", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                // Restart the game
                restartGame();
            }
        });

        builder.setNegativeButton("No", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                // Reset the game state for this player
                if (scoreboard != null && player != null) {
                    restartGame();

                }
                // Navigate to LoginScreen
                Intent intent = new Intent(GameScreen.this, LoginScreen.class);
                startActivity(intent);
                finish();
            }
        });


        AlertDialog dialog = builder.create();
        dialog.show();
        dialog.setCancelable(false);
    }


    private void restartGame() {
        if (dictionary == null) {
            dictionary = new Dictionary(this);
        }

        try {
            String randomWord = dictionary.getRandomWord();
            game = new Game(randomWord);
            wordText.setText(game.getCurrentDisplay());
            wrongGuessLetters.setText(game.getWrongGuesses());
            updateHangmanImage();
        } catch (IllegalStateException e) {
            showNoWordsLeftDialog();
        }
    }


    private void showNoWordsLeftDialog() {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle("No Words Left");
        builder.setMessage("There are no words left in the dictionary.");

        builder.setPositiveButton("OK", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                // Navigate to LoginScreen
                Intent intent = new Intent(GameScreen.this, LoginScreen.class);
                startActivity(intent);
                finish();
            }
        });

        AlertDialog dialog = builder.create();
        dialog.show();
        dialog.setCancelable(false);
    }

    private void updateHangmanImage() {
        int mistakes = game.getMistakes();
        if (mistakes < HANGMAN_IMAGES.length) {
            hangmanImage.setImageResource(HANGMAN_IMAGES[mistakes]);
        } else {
            // Handle the case where mistakes exceed the length of the array
            hangmanImage.setImageResource(HANGMAN_IMAGES[HANGMAN_IMAGES.length - 1]);
        }
    }

    private void useHint() {
        try {
            char hintLetter = game.getHint();
            Toast.makeText(this, "Hint: " + hintLetter, Toast.LENGTH_SHORT).show();
            wordText.setText(game.getCurrentDisplay());
            wrongGuessLetters.setText(game.getWrongGuesses());
            updateHangmanImage();
            checkIfHintAllowed();

            // Check if the word is now fully guessed or if the maximum number of mistakes is reached
            if (game.areAllLettersGuessed()) {
                // Show the end game dialog with a win
                showEndGameDialog(true);
            } else if (game.getMistakes() >= MAX_MISTAKES) {
                // Show the end game dialog with a loss
                showEndGameDialog(false);
            }
        } catch (IllegalStateException e) {
            hintBtn.setEnabled(false); // Disable the hint button
        }
    }


    @Override
    protected void onPause() {
        super.onPause();
        // Save the game state when pausing the activity
        if (scoreboard != null) {
            GameStateManager.saveState(this, game, scoreboard, currentPlayerName);
        }
    }


    private void checkIfHintAllowed() {
        if (game.isOnlyOneLetterLeft()) {
            hintBtn.setEnabled(false);
        } else {
            hintBtn.setEnabled(true);
        }
    }


}

