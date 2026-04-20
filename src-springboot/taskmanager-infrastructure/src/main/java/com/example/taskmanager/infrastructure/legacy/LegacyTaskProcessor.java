package com.example.taskmanager.infrastructure.legacy;

import org.springframework.stereotype.Component;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;

/**
 * LEGACY CODE - This is intentionally bad code for refactoring exercise in Lab 3
 * Anti-patterns included:
 * - Long method with nested conditionals
 * - Poor naming
 * - No logging
 * - Synchronous / blocking code
 * - No error handling
 * - Mixed concerns
 * - Magic numbers and strings
 */
@Component
public class LegacyTaskProcessor {

    public String processTask(int id, String data, int type, boolean flag) {
        var result = "";

        if (data != null) {
            if (data.length() > 0) {
                if (type == 1) {
                    if (flag) {
                        for (int i = 0; i < data.length(); i++) {
                            if (data.charAt(i) == ' ') {
                                result += "_";
                            } else {
                                if (Character.isUpperCase(data.charAt(i))) {
                                    result += Character.toLowerCase(data.charAt(i));
                                } else {
                                    result += Character.toUpperCase(data.charAt(i));
                                }
                            }
                        }

                        if (result.length() > 50) {
                            result = result.substring(0, 50);
                        }

                        // Simulate some processing
                        try {
                            Thread.sleep(100);
                        } catch (InterruptedException e) {
                            // Swallow exception (bad practice)
                        }

                        // Write to file (bad practice - mixed concerns)
                        try {
                            Files.writeString(Path.of("task_" + id + ".txt"), result);
                        } catch (IOException e) {
                            // Swallow exception (bad practice)
                        }
                    } else {
                        result = data.toUpperCase();
                    }
                } else if (type == 2) {
                    var words = data.split(" ");
                    for (int i = 0; i < words.length; i++) {
                        if (i == 0) {
                            result = words[i];
                        } else {
                            result += " " + words[i].toLowerCase();
                        }
                    }
                } else {
                    result = data;
                }
            }
        }

        return result;
    }
}

// TODO: During Lab 3, participants will use Copilot to refactor this into:
// - Multiple focused methods
// - Async implementation (CompletableFuture or reactive)
// - Proper logging with SLF4J Logger
// - Guard clauses instead of nested ifs
