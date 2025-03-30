import subprocess
import threading

def record_video(duration=15, resolution=(1920, 1080), output_file="output_video.h264"):
    # Build the libcamera-vid command
    command = [
        "libcamera-vid",
        "-t", str(duration * 1000),  # Duration in milliseconds
        "--width", str(resolution[0]),
        "--height", str(resolution[1]),
        "-o", output_file
    ]

    # Video recording task
    def record_task():
        try:
            print("Recording video with command:", " ".join(command))
            subprocess.run(command, check=True)
            print(f"Video recording complete! Saved to {output_file}")
        except subprocess.CalledProcessError as e:
            print(f"An error occurred during video recording: {e}")

    # Cleanup task
    def cleanup_task():
        try:
            print(f"No cleanup required for video file: {output_file}")
            # Add cleanup commands here if necessary (e.g., deleting temporary files)
        except Exception as e:
            print(f"An error occurred during cleanup: {e}")

    # Start threads for recording and cleanup
    record_thread = threading.Thread(target=record_task)
    cleanup_thread = threading.Thread(target=cleanup_task)

    record_thread.start()
    record_thread.join()  # Wait for recording to complete
    cleanup_thread.start()
    cleanup_thread.join()  # Perform cleanup afterward

# Example usage
record_video(
    duration=15,            # Video duration (in seconds)
    resolution=(1280, 720),  # Resolution (720p)
    output_file="video.h264" # Output video filename
)