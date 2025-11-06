package com.example.hangmangame;

import androidx.appcompat.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.TextView;
import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.Scanner;

public class HelpScreen extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_help_screen);

        TextView rulesTextView = findViewById(R.id.rulesTextView);
        String rules = readFromRawResource(R.raw.rules);
        rulesTextView.setText(rules);
    }

    private String readFromRawResource(int resourceId) {
        InputStream inputStream = getResources().openRawResource(resourceId);
        Scanner scanner = new Scanner(inputStream);
        StringBuilder result = new StringBuilder();
        int lineNumber = 1; // Start from 1 for numbered list

        while (scanner.hasNextLine()) {
            String line = scanner.nextLine();

            // For numbered list, uncomment the following lines:
             result.append(lineNumber).append(". ").append(line).append("\n");
             lineNumber++;

            // Append extra newline for better readability
            if (!line.trim().isEmpty()) {
                result.append("\n");
            }
        }

        scanner.close();
        return result.toString();
    }


}
