input_filename = "output2"    # Replace with the name of your input file
output_filename = "output3"  # Replace with the name of your output file

# Open the input and output files
with open(input_filename, "r") as input_file, open(output_filename, "w") as output_file:
    cumulative_sum = 0.0  # Initialize the cumulative sum

    # Iterate through each line in the input file
    for line in input_file:
        try:
            # Convert the line to a float
            value = float(line.strip())

            # Update the cumulative sum
            cumulative_sum += value

            # Write the cumulative sum to the output file
            output_file.write(f"{cumulative_sum:.2f}\n")  # Assuming 2 decimal places
        except ValueError:
            # Handle invalid input (non-float lines)
            print(f"Skipping line: {line.strip()}")

# Close the files
input_file.close()
output_file.close()

print("Cumulative sum calculation complete. Results saved to", output_filename)

