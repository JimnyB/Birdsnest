from flask import Flask, Response
from picamera2 import Picamera2
import cv2

app = Flask(__name__)
camera = None  # Global Picamera2 object

def start_feed():
    global camera
    if not camera:
        try:
            # Initialize libcamera
            camera = Picamera2()
            camera.configure(camera.create_preview_configuration())
            camera.start()
        except Exception as e:
            camera = None
            return f"Failed to start the camera: {str(e)}"
    return "Camera started."

def stop_feed():
    global camera
    if camera:
        try:
            camera.stop()
            camera = None
        except Exception as e:
            return f"Failed to stop the camera: {str(e)}"
    return "Camera stopped."

def generate_frames():
    global camera
    if not camera:
        return  # Camera not initialized

    while True:
        try:
            # Capture a frame
            frame = camera.capture_array()
            # Flip the frame vertically
            frame = cv2.flip(frame, 0)  # Flip vertically (use 1 for horizontal, or -1 for both)
            _, buffer = cv2.imencode('.jpg', frame)  # Convert to JPEG
            frame = buffer.tobytes()
            yield (b'--frame\r\n'
                   b'Content-Type: image/jpeg\r\n\r\n' + frame + b'\r\n')
        except Exception as e:
            print(f"Error capturing frame: {e}")
            break

@app.route('/start_feed', methods=['GET'])
def handle_start_feed():
    return start_feed()

@app.route('/stop_feed', methods=['GET'])
def handle_stop_feed():
    return stop_feed()

@app.route('/video_feed')
def video_feed():
    global camera
    if not camera:
        return "Camera not started."
    return Response(generate_frames(), mimetype='multipart/x-mixed-replace; boundary=frame')

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000)