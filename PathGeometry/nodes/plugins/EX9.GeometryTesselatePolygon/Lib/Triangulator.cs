using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using VVVV.Core.Logging;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.EX9;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

namespace VVVV.Lib 
{
    
public class Triangulator
{
    private List<Vector2D> m_points = new List<Vector2D>();
    
    public Triangulator (ISpread<Vector2D> points) {
        m_points = new List<Vector2D>(points);
    }
    
    public int[] Triangulate() {
        List<int> indices = new List<int>();
        
        int n = m_points.Count;
        if (n < 3)
            return indices.ToArray();
        
        int[] V = new int[n];
        if (Area() > 0) {
            for (int v = 0; v < n; v++)
                V[v] = v;
        }
        else {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }
        
        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2; ) {
            if ((count--) <= 0)
                return indices.ToArray();
            
            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;
            
            if (Snip(u, v, w, nv, V)) {
                int a, b, c, s, t;
                a = V[u];
                b = V[v];
                c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                m++;
                for (s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }
        
        indices.Reverse();
        return indices.ToArray();
    }
    
    private double Area () {
        int n = m_points.Count;
        double A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++) {
            Vector2D pval = m_points[p];
            Vector2D qval = m_points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5d);
    }
    
    private bool Snip (int u, int v, int w, int n, int[] V) {
        int p;
        Vector2D A = m_points[V[u]];
        Vector2D B = m_points[V[v]];
        Vector2D C = m_points[V[w]];
        if (0.00000001f > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;
        for (p = 0; p < n; p++) {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2D P = m_points[V[p]];
            if (InsideTriangle(A, B, C, P))
                return false;
        }
        return true;
    }
    
    private bool InsideTriangle (Vector2D A, Vector2D B, Vector2D C, Vector2D P) {
        double a_x, a_y, b_x, b_y, c_x, c_y, apx, apy, bpx, bpy, cpx, cpy;
        double cCROSSap, bCROSScp, aCROSSbp;
        
        a_x = C.x - B.x; a_y = C.y - B.y;
        b_x = A.x - C.x; b_y = A.y - C.y;
        c_x = B.x - A.x; c_y = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;
        
        aCROSSbp = a_x * bpy - a_y * bpx;
        cCROSSap = c_x * apy - c_y * apx;
        bCROSScp = b_x * cpy - b_y * cpx;
        
        return ((aCROSSbp >= 0.0d) && (bCROSScp >= 0.0d) && (cCROSSap >= 0.0d));
    }
}
}