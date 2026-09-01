"""Legacy task processing code used for the Lab 3 refactoring exercise."""

from time import sleep


def process_task(id, data, type, flag):
    result = ""

    if data is not None:
        if len(data) > 0:
            if type == 1:
                if flag:
                    i = 0
                    while i < len(data):
                        ch = data[i]
                        if ch == " ":
                            result = result + "_"
                        else:
                            if ch.isupper():
                                result = result + ch.lower()
                            else:
                                result = result + ch.upper()
                        i = i + 1

                    if len(result) > 50:
                        result = result[:50]

                    sleep(0.1)
                    f = open(f"task_{id}.txt", "w", encoding="utf-8")
                    f.write(result)
                    f.close()
                else:
                    result = data.upper()
            else:
                if type == 2:
                    words = data.split(" ")
                    x = 0
                    while x < len(words):
                        if x == 0:
                            result = words[x]
                        else:
                            result = result + " " + words[x].lower()
                        x = x + 1
                else:
                    if type == 3:
                        result = data.strip() + "-" + str(id)
                        sleep(0.05)
                        print("saved", result)
                    else:
                        result = data

    return result


# TODO: During Lab 3, participants will use Copilot to refactor this into:
# - Multiple focused functions
# - Type hints and clear parameter names
# - Guard clauses instead of nested if statements
# - Proper logging with the standard logging module
# - Separation of formatting, persistence, and side effects
