using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2___2D_Circle_Area___Niederreiter_QRNG
{
    class Niederreiter
    {
        const int MAXDEG = 50;
        const int DIM_MAX = 20;
        const int NBITS = 31;

        static void setfld2(int[,] add, int[,] mul, int[,] sub)
        {
            int i;
            int j;
            int p = 2;
            int q = 2;

            for (i = 0; i < q; i++)
            {
                for (j = 0; j < q; j++)
                {
                    add[i, j] = (i + j) % p;
                    mul[i, j] = (i * j) % p;
                }
            }
            for (i = 0; i < q; i++)
            {
                for (j = 0; j < q; j++)
                {
                    sub[add[i, j], i] = j;
                }
            }
        }

        static void plymul2(int[,] add, int[,] mul, int pa_deg, int[] pa, int pb_deg, int[] pb, ref int pc_deg, int[] pc)
        {
            int i;
            int j;
            int jhi;
            int jlo;
            int[] pt = new int[MAXDEG + 1];
            int term;

            if (pa_deg == -1 || pb_deg == -1)
            {
                pc_deg = -1;
            }
            else
            {
                pc_deg = pa_deg + pb_deg;
            }

            if (MAXDEG < pc_deg)
            {
                Exception e = new Exception("PLYMUL2 - Fatal error!\nDegree of the product exceeds MAXDEG.\n");
                throw (e);
            }

            for (i = 0; i <= pc_deg; i++)
            {

                jlo = i - pa_deg;
                if (jlo < 0)
                {
                    jlo = 0;
                }

                jhi = pb_deg;
                if (i < jhi)
                {
                    jhi = i;
                }

                term = 0;

                for (j = jlo; j <= jhi; j++)
                {
                    term = add[term, mul[pa[i - j], pb[j]]];
                }
                pt[i] = term;
            }

            for (i = 0; i <= pc_deg; i++)
            {
                pc[i] = pt[i];
            }

            for (i = pc_deg + 1; i <= MAXDEG; i++)
            {
                pc[i] = 0;
            }
        }

        static void calcc2(int dim_num, int[,] cj)
        {
            int MAXE = 6;
            int[,] add = new int[2, 2];
            int[] b = new int[MAXDEG + 1];
            int b_deg;
            int[,] ci = new int[NBITS, NBITS];
            int e;
            int i;
            int[,] irred = { { 0,1,0,0,0,0,0 },  // DIM_MAX x MAXE + 1
                             { 1,1,0,0,0,0,0 },
                             { 1,1,1,0,0,0,0 },
                             { 1,1,0,1,0,0,0 },
                             { 1,0,1,1,0,0,0 },
                             { 1,1,0,0,1,0,0 },
                             { 1,0,0,1,1,0,0 },
                             { 1,1,1,1,1,0,0 },
                             { 1,0,1,0,0,1,0 },
                             { 1,0,0,1,0,1,0 },
                             { 1,1,1,1,0,1,0 },
                             { 1,1,1,0,1,1,0 },
                             { 1,1,0,1,1,1,0 },
                             { 1,0,1,1,1,1,0 },
                             { 1,1,0,0,0,0,1 },
                             { 1,0,0,1,0,0,1 },
                             { 1,1,1,0,1,0,1 },
                             { 1,1,0,1,1,0,1 },
                             { 1,0,0,0,0,1,1 },
                             { 1,1,1,0,0,1,1 } };

            int[] irred_deg = { 1, 1, 2, 3, 3, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6 }; // DIM_MAX
            int j;
            int maxv = NBITS + MAXE;
            int[,] mul = new int[2, 2];
            int[] nextq = new int[DIM_MAX];
            int p;
            int[] px = new int[MAXDEG + 1];
            int px_deg;
            int q;
            int r;
            int[,] sub = new int[2, 2];
            int term;
            int u;
            int[] v = new int[NBITS + MAXE + 1];

            setfld2(add, mul, sub);

            for (i = 0; i < dim_num; i++)
            {
                e = irred_deg[i];
                px_deg = irred_deg[i];
                for (j = 0; j <= px_deg; j++)
                {
                    px[j] = irred[i, j];
                }
                b_deg = 0;
                b[0] = 1;
                u = 0;
                for (j = 0; j < NBITS; j++)
                {
                    if (u == 0)
                    {
                        calcv2(maxv, px_deg, px, add, mul, sub, ref b_deg, b, v);
                    }
                    for (r = 0; r < NBITS; r++)
                    {
                        ci[j, r] = v[r + u];
                    }
                    u = u + 1;
                    if (u == e)
                    {
                        u = 0;
                    }
                }
                for (r = 0; r < NBITS; r++)
                {
                    term = 0;
                    for (j = 0; j < NBITS; j++)
                    {
                        term = 2 * term + ci[j, r];
                    }
                    cj[i, r] = term;
                }

            }
        }

        static void calcv2(int maxv, int px_deg, int[] px, int[,] add, int[,] mul, int[,] sub, ref int b_deg, int[] b, int[] v)
        {
            int arbit = 1;
            int bigm;
            int e;
            int[] h = new int[MAXDEG + 1];
            int h_deg;
            int i;
            int j;
            int kj;
            int m;
            int nonzer = 1;
            int p = 2;
            int pb_deg;
            int q = 2;
            int r;
            int term;

            e = px_deg;
            h_deg = b_deg;

            for (i = 0; i <= h_deg; i++)
            {
                h[i] = b[i];
            }

            bigm = h_deg;

            pb_deg = b_deg;

            plymul2(add, mul, px_deg, px, pb_deg, b, ref pb_deg, b);

            b_deg = pb_deg;
            m = b_deg;

            j = m / e;

            kj = bigm;
            for (r = 0; r < kj; r++)
            {
                v[r] = 0;
            }
            v[kj] = 1;

            if (kj < bigm)
            {
                term = sub[0, h[kj]];

                for (r = kj + 1; r <= bigm - 1; r++)
                {
                    v[r] = arbit;
                    term = sub[term, mul[h[r], v[r]]];

                }
                v[bigm] = add[nonzer, term];

                for (r = bigm + 1; r <= m - 1; r++)
                {
                    v[r] = arbit;
                }
            }
            else
            {
                for (r = kj + 1; r <= m - 1; r++)
                {
                    v[r] = arbit;
                }

            }
            for (r = 0; r <= maxv - m; r++)
            {
                term = 0;
                for (i = 0; i <= m - 1; i++)
                {
                    term = sub[term, mul[b[i], v[r + i]]];
                }
                v[r + m] = term;
            }

            return;
        }

        public static void niederreiter2(int dim_num, ref int seed, double[] quasi)
        {
            const double RECIP = 1.0 / (double)(1 << NBITS);

            int[,] cj = new int[DIM_MAX, NBITS];
            int dim_save = 0;
            int gray;
            int i;
            int[] nextq = new int[DIM_MAX];
            int r;
            int skip;
            int seed_save = 0;
            if (dim_save < 1 || dim_num != dim_save || seed <= 0)
            {
                if (dim_num <= 0 || DIM_MAX < dim_num)
                {
                    Exception e = new Exception("NIEDERREITER2 - Fatal error!\nBad spatial dimension.\n");
                    throw (e);
                }

                dim_save = dim_num;

                if (seed < 0)
                {
                    seed = 0;
                }

                seed_save = seed;

                calcc2(dim_save, cj);
            }
            if (seed != seed_save + 1)
            {
                gray = (seed) ^ (seed / 2);

                for (i = 0; i < dim_save; i++)
                {
                    nextq[i] = 0;
                }

                r = 0;

                while (gray != 0)
                {
                    if ((gray % 2) != 0)
                    {
                        for (i = 0; i < dim_save; i++)
                        {
                            nextq[i] = (nextq[i]) ^ (cj[i, r]);
                        }
                    }
                    gray = gray / 2;
                    r = r + 1;
                }
            }

            for (i = 0; i < dim_save; i++)
            {
                quasi[i] = ((double)nextq[i]) * RECIP;
            }

            r = 0;
            i = seed;

            while ((i % 2) != 0)
            {
                r = r + 1;
                i = i / 2;
            }

            if (NBITS <= r)
            {
                Exception e = new Exception("NIEDERREITER2 - Fatal error!\nToo many calls!\n");
                throw (e);
            }

            for (i = 0; i < dim_save; i++)
            {
                nextq[i] = (nextq[i]) ^ (cj[i, r]);
            }

            seed_save = seed;
            seed = seed + 1;
        }

        public static double[] niederreiter2_generate(int dim_num, int n, ref int seed)
        {
            int j;
            double[] r = new double[dim_num * n];

            for (j = 0; j < n; j++)
            {
                niederreiter2(dim_num, ref seed, r.Skip(j * dim_num).ToArray());
            }

            return r;
        }

    }
}
