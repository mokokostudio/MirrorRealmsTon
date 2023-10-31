using System;

public struct SwitchEnum<T> where T : Enum {
    public T white;
    public T black;

    public bool CheckWhite(T value) => (Convert.ToInt32(white) & Convert.ToInt32(value)) != 0;
    public bool CheckBlack(T value) => (Convert.ToInt32(black) & Convert.ToInt32(value)) != 0;
    public bool CheckPass(T value) => CheckWhite(value) && !CheckBlack(value);
}
