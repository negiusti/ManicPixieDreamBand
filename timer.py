import time

def main():
    print("Press any key to start. Press 'q' to quit.")
    
    start_time = None
    time_diffs = []
    
    while True:
        key = input()
        
        if key == 'q':
            break
        
        current_time = time.time()

        if start_time is not None:
            elapsed_time = current_time - start_time
            print(f"Time since last 1 key press: {elapsed_time:.4f} seconds")
            time_diffs.append(elapsed_time)    
        start_time = current_time
    print (time_diffs)


if __name__ == "__main__":
    main()
