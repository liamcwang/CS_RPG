public class ImGuiController {
    private int _windowWidth;
    private int _windowHeight;

    public void WindowResized(int width, int height) {
        _windowWidth = width;
        _windowHeight = height;
    }
}