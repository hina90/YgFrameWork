using System;

public class TextUnpacker
{
    private string m_text;
    private int m_pos;

    public TextUnpacker(string text)
    {
        this.m_text = text;
        this.m_pos = 0;
    }

    public bool IsEmpty()
    {
        return m_pos >= m_text.Length;
    }

    public bool IsDelimiter(char delimiter = ',')
    {
        return m_pos > 0 && m_text[m_pos - 1] == delimiter;
    }

    public bool IsAnchor(char anchor = ';')
    {
        if (m_pos < m_text.Length && m_text[m_pos] == anchor)
        {
            ++m_pos;
            return true;
        }
        return false;
    }

    public Int32 UnpackInt32()
    {
        Int32 value = 0;
        int stop = findIntegerStop(m_text, m_pos);
        Int32.TryParse(m_text.Substring(m_pos, stop - m_pos), out value);
        m_pos = stop + 1;
        return value;
    }

    public UInt32 UnpackUInt32()
    {
        UInt32 value = 0;
        int stop = findIntegerStop(m_text, m_pos);
        UInt32.TryParse(m_text.Substring(m_pos, stop - m_pos), out value);
        m_pos = stop + 1;
        return value;
    }

    public Int64 UnpackInt64()
    {
        Int64 value = 0;
        int stop = findIntegerStop(m_text, m_pos);
        Int64.TryParse(m_text.Substring(m_pos, stop - m_pos), out value);
        m_pos = stop + 1;
        return value;
    }

    public UInt64 UnpackUInt64()
    {
        UInt64 value = 0;
        int stop = findIntegerStop(m_text, m_pos);
        UInt64.TryParse(m_text.Substring(m_pos, stop - m_pos), out value);
        m_pos = stop + 1;
        return value;
    }

    public float UnpackFloat()
    {
        float value = 0;
        int stop = findFloatStop(m_text, m_pos);
        float.TryParse(m_text.Substring(m_pos, stop - m_pos), out value);
        m_pos = stop + 1;
        return value;
    }

    public double UnpackDouble()
    {
        double value = 0;
        int stop = findFloatStop(m_text, m_pos);
        double.TryParse(m_text.Substring(m_pos, stop - m_pos), out value);
        m_pos = stop + 1;
        return value;
    }

    public string UnpackString()
    {
        string value = string.Empty;
        var length = UnpackInt32();
        if (length > 0)
        {
            if (m_pos + length + 1 <= m_text.Length)
            {
                value = m_text.Substring(m_pos, length);
                m_pos += length + 1;
            }
            else
            {
                value = m_text.Substring(m_pos);
                m_pos = m_text.Length;
            }
        }
        return value;
    }

    static private int findNumberStop(string text, int start)
    {
        int pos = start;
        for (int len = text.Length; pos < len; ++pos)
        {
            char ch = text[pos];
            if (ch < '0' || ch > '9')
            {
                break;
            }
        }
        return pos;
    }

    static private int findIntegerStop(string text, int start)
    {
        return findNumberStop(text, start + (isSign(text, start) ? 1 : 0));
    }

    static private int findFloatStop(string text, int start)
    {
        int pos = findIntegerStop(text, start);
        if (isDot(text, pos))
        {
            pos = findNumberStop(text, pos + 1);
        }
        if (isExp(text, pos))
        {
            pos = findIntegerStop(text, pos + 1);
        }
        return pos;
    }

    static private bool isSign(string text, int pos)
    {
        if (pos < text.Length)
        {
            char ch = text[pos];
            return ch == '+' || ch == '-';
        }
        else
        {
            return false;
        }
    }
    static private bool isExp(string text, int pos)
    {
        if (pos < text.Length)
        {
            char ch = text[pos];
            return ch == 'e' || ch == 'E';
        }
        else
        {
            return false;
        }
    }
    static private bool isDot(string text, int pos)
    {
        if (pos < text.Length)
        {
            char ch = text[pos];
            return ch == '.';
        }
        else
        {
            return false;
        }
    }
};
