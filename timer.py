import playsound
import time
import pyaudio
import io
import sounddevice as sd

# sorry, it's a mess in here :)

def play_wav_file(file_path):
    # Read the WAV file and get the audio data
    wave_data, sample_rate = sd.read(file_path, dtype='int16')

    # Play the audio data
    sd.play(wave_data, sample_rate)
    sd.wait()
'''def play_audio(file_path):
    p = pyaudio.PyAudio()

    wf = open(file_path, "rb")
    stream = p.open(format=p.get_format_from_width(wf.getsampwidth()),
                    channels=wf.getnchannels(),
                    rate=wf.getframerate(),
                    output=True)
    
    data = wf.readframes(1024)

    while data:
        stream.write(data)
        data = wf.readframes(1024)

    stream.stop_stream()
    stream.close()

    p.terminate()'''

import wave

def play_wav_file2(file_path):
    chunk = 1024
    wf = wave.open(file_path, 'rb')

    p = pyaudio.PyAudio()

    stream = p.open(format=p.get_format_from_width(wf.getsampwidth()),
                    channels=wf.getnchannels(),
                    rate=wf.getframerate(),
                    output=True)

    data = wf.readframes(chunk)

    while data:
        stream.write(data)
        data = wf.readframes(chunk)

    stream.stop_stream()
    stream.close()

    p.terminate()

def play():
    playsound.playsound('HamsterBaby.wav',False)
    # play_wav_file("HamsterBaby.wav")

def main():
    # Press any key to start. Press 'q' to quit
    audio_file = "Hamster Baby.m4a" 
    
    start_time = None
    time_diffs = []
    first = True
    while True:
        key = input()
        if first:
            play()
            first = False

        if key == 'q':
            break
        
        current_time = time.time()

        if start_time is not None:
            elapsed_time = current_time - start_time
#            print(f"Time since last 1 key press: {elapsed_time:.4f} seconds")
            time_diffs.append(elapsed_time)    
        start_time = current_time
    i = 0
    while i < len(time_diffs):
      print (time_diffs[i])
      i += 1


if __name__ == "__main__":
    main()
