using AIEngine;

namespace Tic_Tac_Toe
{
    public static class Heuristic
    {

        public static int BoardSize { get; set; }
        public static int LineLenght { get; set; }


        const int FULL_THREAT_SEQUENCE = 1000;  //N           O O O O
        const int OPENED_THREAT_SEQUENCE = 600;  //N-1        . O O O . 
        const int BROKEN_THREAT_SEQUENCE = 40;  //N-1         O O . O   or O . O O
        const int HALF_OPENED_THREAT_SEQUENCE = 60;  //N-1    X O O O . or . O O O X     
        const int WEACK_THREAT_SEQUENCE1 = 20;  //N-2         . . O O . . 
        const int WEACK_THREAT_SEQUENCE2 = 10;  //N-2          . O O . X    or   X . O O .
        const int WEACK_THREAT_SEQUENCE3 = 5;  //N-2           . . O O  X    or   X O O . .




        public static int GetHorizantalThreatSequences(INode n)
        {
            var b = n as Board;

            char state = Gomoku.NONE;

            int score = 0;

            bool isHalfOpenLine;

            int i,
                j,
                s_count,// current threat sequence length
                j_s,    //threat sequence X start
                j_e;    //threat sequence X end


            i = j = 0;
            bool done = false;

            while (!done)
            {
                //searching for a computer state
                while (b[i, j] == Gomoku.NONE)
                {
                    j++;
                    if (j == BoardSize)
                    {
                        j = 0;
                        i++;
                        if (i == BoardSize)
                        {
                            done = true;
                            break;
                        }
                    }
                }

                if (done) break;

                state = b[i, j]; // computer or player state
                j_s = j;  //save the sequence start x coordiante 
                //if (BoardSize - i_s < LineLenght)// if we don't have enaugh space to aligne a line we move to the next line

                //once we find it, count if we have a full threat sequence 
                s_count = 0;
                while (b[i, j] == state && s_count < LineLenght)
                {
                    s_count++;
                    j++;
                    if (j == BoardSize)
                    {
                        j = 0;
                        i++;

                        if (i == BoardSize)
                        {
                            i = BoardSize - 1;
                            done = true;
                        }
                        break;
                    }
                }

                j_e = (j == 0) ? BoardSize - 1 : j - 1;

                isHalfOpenLine = false;
                if (s_count == LineLenght)// we have a full threat sequence
                    score += (state == Gomoku.COMPUTER) ? FULL_THREAT_SEQUENCE : -FULL_THREAT_SEQUENCE;
                else
                {
                    if (s_count == LineLenght - 1)
                    {
                        //check if we have a half opened threat sequence    . O O O X  || X O O O . 
                        if (j_e != BoardSize - 1)
                        {
                            if (b[i, j_e + 1] == Gomoku.NONE)
                            {
                                score += (state == Gomoku.COMPUTER) ? HALF_OPENED_THREAT_SEQUENCE : -HALF_OPENED_THREAT_SEQUENCE + 1;
                                isHalfOpenLine = true;// first limit is empty  ? O O O .                                
                            }
                        }
                        if (j_s > 0)
                        {
                            if (b[i, j_s - 1] == Gomoku.NONE)
                            {
                                if (isHalfOpenLine)
                                {
                                    score += (state == Gomoku.COMPUTER) ? OPENED_THREAT_SEQUENCE - HALF_OPENED_THREAT_SEQUENCE : HALF_OPENED_THREAT_SEQUENCE - OPENED_THREAT_SEQUENCE;

                                }

                                ////check for a broken threat sequence  O . O O O 
                                //if (j_s > 1 && b[i,j_s - 2] == state)
                                //{
                                //    score += (state == Gomoku.COMPUTER) ?  + BROKEN_THREAT_SEQUENCE : - BROKEN_THREAT_SEQUENCE;
                                //}
                            }
                        }
                    }
                    else if (s_count == LineLenght - 2)
                    {

                        if ((j_e < BoardSize - 2 && j_s > 1) && (b[i, j_e + 1] == Gomoku.NONE && b[i, j_e + 2] == Gomoku.NONE)
                             && b[i, j_s - 1] == Gomoku.NONE && b[i, j_s - 2] == Gomoku.NONE) // . . O O . .
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE1 : -WEACK_THREAT_SEQUENCE1;
                        }
                        else
                        if (
                            ((j_e < BoardSize - 2 && j_s > 0) && (b[i, j_e + 1] == Gomoku.NONE && b[i, j_e + 2] == Gomoku.NONE && b[i, j_s - 1] == Gomoku.NONE))
                            ||
                            ((j_e < BoardSize - 1 && j_s > 1) && (b[i, j_s - 1] == Gomoku.NONE && b[i, j_s - 2] == Gomoku.NONE && b[i, j_e + 1] == Gomoku.NONE))
                            )
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE2 : -WEACK_THREAT_SEQUENCE2;
                        }
                        else
                        if (
                            ((j_e < BoardSize - 2) && (b[i, j_e + 1] == Gomoku.NONE && b[i, j_e + 2] == Gomoku.NONE))
                            ||
                            ((j_s > 1) && (b[i, j_s - 1] == Gomoku.NONE && b[i, j_s - 2] == Gomoku.NONE))
                            )
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE3 : -WEACK_THREAT_SEQUENCE3;
                        }
                        else
                            //check for a broken threat sequence  O O O . O 
                            if (j_e < BoardSize - 2 && b[i, j_e + 2] == state && b[i, j_e + 1] == Gomoku.NONE)
                        {
                            score += (state == Gomoku.COMPUTER) ? BROKEN_THREAT_SEQUENCE : -BROKEN_THREAT_SEQUENCE;
                        }
                        else
                                //check for a broken threat sequence  O . O O 
                                if (j_s > 1 && b[i, j_s - 2] == state && b[i, j_s - 1] == Gomoku.NONE)
                        {
                            score += (state == Gomoku.COMPUTER) ? BROKEN_THREAT_SEQUENCE : -BROKEN_THREAT_SEQUENCE;
                        }

                    }
                }
            }//while



            return score;
        }


        public static int GetVerticalThreatSequences(INode n)
        {


            var b = n as Board;
            char state = Gomoku.NONE;
            int score = 0;
            bool isHalfOpenLine;
            int j,
                i,
                s_length,// current threat sequence length
                i_s,    //threat sequence X start
                i_e;    //threat sequence X end


            j = i = 0;
            bool done = false;

            while (!done)
            {
                //searching for a computer state
                while (b[i, j] == Gomoku.NONE)
                {
                    i++;
                    if (i == BoardSize)
                    {
                        i = 0;
                        j++;
                        if (j == BoardSize)
                        {
                            done = true;
                            break;
                        }
                    }
                }

                if (done) break;

                state = b[i, j]; // computer or player state
                i_s = i;  //save the sequence start x coordiante 
                //if (BoardSize - i_s < LineLenght)// if we don't have enaugh space to aligne a line we move to the next line

                //once we find it, count if we have a full threat sequence 
                s_length = 0;
                while (b[i, j] == state && s_length < LineLenght)
                {
                    s_length++;
                    i++;
                    if (i == BoardSize)
                    {
                        i = 0;
                        j++;

                        if (j == BoardSize)
                        {
                            j = BoardSize - 1;
                            done = true;
                        }
                        break;
                    }
                }

                i_e = (i == 0) ? BoardSize - 1 : i - 1;

                isHalfOpenLine = false;
                if (s_length == LineLenght)// we have a full threat sequence
                    score += (state == Gomoku.COMPUTER) ? FULL_THREAT_SEQUENCE : -FULL_THREAT_SEQUENCE;
                else
                {
                    if (s_length == LineLenght - 1)
                    {
                        //check if we have a half opened threat sequence    . O O O X  || X O O O . 
                        if (i_e != BoardSize - 1)
                        {
                            if (b[i_e + 1, j] == Gomoku.NONE)
                            {
                                score += (state == Gomoku.COMPUTER) ? HALF_OPENED_THREAT_SEQUENCE : -HALF_OPENED_THREAT_SEQUENCE + 1;
                                isHalfOpenLine = true;// first limit is empty  ? O O O .

                            }
                        }
                        if (i_s > 0)
                        {
                            if (b[i_s - 1, j] == Gomoku.NONE)
                            {
                                if (isHalfOpenLine)
                                {
                                    score += (state == Gomoku.COMPUTER) ? OPENED_THREAT_SEQUENCE - HALF_OPENED_THREAT_SEQUENCE : HALF_OPENED_THREAT_SEQUENCE - OPENED_THREAT_SEQUENCE;
                                }
                            }
                        }
                    }
                    else if (s_length == LineLenght - 2)
                    {

                        if ((i_e < BoardSize - 2 && i_s > 1) && (b[i_e + 1, j] == Gomoku.NONE && b[i_e + 2, j] == Gomoku.NONE)
                             && b[i_s - 1, j] == Gomoku.NONE && b[i_s - 2, j] == Gomoku.NONE) // . . O O . .
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE1 : -WEACK_THREAT_SEQUENCE1;
                        }
                        else
                            if (
                                ((i_e < BoardSize - 2 && i_s > 0) && (b[i_e + 1, j] == Gomoku.NONE && b[i_e + 2, j] == Gomoku.NONE && b[i_s - 1, j] == Gomoku.NONE))
                                ||
                                ((i_e < BoardSize - 1 && i_s > 1) && (b[i_s - 1, j] == Gomoku.NONE && b[i_s - 2, j] == Gomoku.NONE && b[i_e + 1, j] == Gomoku.NONE))
                                )
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE2 : -WEACK_THREAT_SEQUENCE2;
                        }
                        else
                                if (
                                    ((i_e < BoardSize - 2) && (b[i_e + 1, j] == Gomoku.NONE && b[i_e + 2, j] == Gomoku.NONE))
                                    ||
                                    ((i_s > 1) && (b[i_s - 1, j] == Gomoku.NONE && b[i_s - 2, j] == Gomoku.NONE))
                                    )
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE3 : -WEACK_THREAT_SEQUENCE3;
                        }

                        else
                                    //check for a broken threat sequence  O O O . O 
                                    if (i_e < BoardSize - 2 && b[i_e + 2, j] == state && b[i_e + 1, j] == Gomoku.NONE)
                        {
                            score += (state == Gomoku.COMPUTER) ? BROKEN_THREAT_SEQUENCE : -BROKEN_THREAT_SEQUENCE;
                        }
                        else
                                        //check for a broken threat sequence  O . O O 
                                        if (i_s > 1 && b[i_s - 2, j] == state && b[i_s - 1, j] == Gomoku.NONE)
                        {
                            score += (state == Gomoku.COMPUTER) ? BROKEN_THREAT_SEQUENCE : -BROKEN_THREAT_SEQUENCE;
                        }

                    }
                }

            }//while

            return score;
        }



        public static int GetDiagonalThreatSequences1(INode n)
        {


            var b = n as Board;
            char state = Gomoku.NONE;
            int score = 0;
            bool isHalfOpenLine;
            int j,
                i,
                s_length,// current threat sequence length
                i_s,    //threat sequence X start
                i_e,    //threat sequence X end
                j_s,    //threat sequence Y start
                j_e;    //threat sequence Y end


            j = i = 0;

            bool swich = false;
            for (int jj = 0; jj < BoardSize - LineLenght; jj++)
            {
                if (jj == BoardSize - LineLenght)
                {
                    if (swich) break;
                    swich = true;
                    jj = 0;
                }
                if (swich)
                {
                    i = jj;
                    j = 0;
                }
                else
                {
                    i = 0;
                    j = jj;
                }
                //searching for a computer state
                while (b[i, j] == Gomoku.NONE)
                {
                    i++;
                    j++;
                    if (i == BoardSize || j == BoardSize)
                        break;
                }
                if (i == BoardSize || j == BoardSize) continue;
                state = b[i, j]; // computer or player state
                i_s = i;  //save the sequence start x coordiante 
                j_s = j;
                //once we find it, count if we have a full threat sequence 
                s_length = 0;
                while (b[i, j] == state && s_length < LineLenght)
                {
                    s_length++;
                    i++;
                    j++;
                    if (i == BoardSize || j == BoardSize)
                    {
                        i--;
                        j--;
                        break;
                    }
                }

                i_e = i;
                j_e = j;

                isHalfOpenLine = false;
                if (s_length == LineLenght)// we have a full threat sequence
                    score += (state == Gomoku.COMPUTER) ? FULL_THREAT_SEQUENCE : -FULL_THREAT_SEQUENCE;
                else
                {
                    if (s_length == LineLenght - 1)
                    {
                        //check if we have a half opened threat sequence    . O O O X  || X O O O . 
                        if (i_e != BoardSize - 1 && j_e != BoardSize - 1)
                        {
                            if (b[i_e + 1, j_e + 1] == Gomoku.NONE)
                            {
                                score += (state == Gomoku.COMPUTER) ? HALF_OPENED_THREAT_SEQUENCE : -HALF_OPENED_THREAT_SEQUENCE + 1;
                                isHalfOpenLine = true;// first limit is empty  ? O O O .

                            }
                        }
                        if (i_s > 0 && j_s > 0)
                        {
                            if (b[i_s - 1, j_s - 1] == Gomoku.NONE)
                            {
                                if (isHalfOpenLine)
                                {
                                    score += (state == Gomoku.COMPUTER) ? OPENED_THREAT_SEQUENCE - HALF_OPENED_THREAT_SEQUENCE : HALF_OPENED_THREAT_SEQUENCE - OPENED_THREAT_SEQUENCE;
                                }
                            }
                        }
                    }
                    else if (s_length == LineLenght - 2)
                    {

                        if ((i_e < BoardSize - 2 && i_s > 1) && (j_e < BoardSize - 2 && j_s > 1) &&
                            (b[i_e + 1, j_e + 1] == Gomoku.NONE && b[i_e + 2, j_e + 2] == Gomoku.NONE)
                             && b[i_s - 1, j_s - 1] == Gomoku.NONE && b[i_s - 2, j_s - 2] == Gomoku.NONE) // . . O O . .
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE1 : -WEACK_THREAT_SEQUENCE1;
                        }
                        else
                            if (
                                ((i_e < BoardSize - 2 && i_s > 0) && (j_e < BoardSize - 2 && j_s > 0) &&
                                (b[i_e + 1, j_e + 1] == Gomoku.NONE && b[i_e + 2, j_e + 2] == Gomoku.NONE && b[i_s - 1, j_s - 1] == Gomoku.NONE))
                                ||
                                ((i_e < BoardSize - 1 && i_s > 1) && (j_e < BoardSize - 1 && j_s > 1) &&
                                (b[i_s - 1, j_s - 1] == Gomoku.NONE && b[i_s - 2, j_s - 2] == Gomoku.NONE && b[i_e + 1, j_e + 1] == Gomoku.NONE))
                                )
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE2 : -WEACK_THREAT_SEQUENCE2;
                        }
                        else
                        if (
                            ((i_e < BoardSize - 2) && (j_e < BoardSize - 2) &&
                            (b[i_e + 1, j_e + 1] == Gomoku.NONE && b[i_e + 2, j_e + 2] == Gomoku.NONE))
                            ||
                            ((i_s > 1) && (j_s > 1) && (b[i_s - 1, j_s - 1] == Gomoku.NONE && b[i_s - 2, j_s - 2] == Gomoku.NONE))
                            )
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE3 : -WEACK_THREAT_SEQUENCE3;
                        }

                        else
                            //check for a broken threat sequence  O O O . O 
                            if (i_e < BoardSize - 2 && j_e < BoardSize - 2 && b[i_e + 2, j_e + 2] == state && b[i_e + 1, j_e + 1] == Gomoku.NONE)
                        {
                            score += (state == Gomoku.COMPUTER) ? BROKEN_THREAT_SEQUENCE : -BROKEN_THREAT_SEQUENCE;
                        }
                        else
                                //check for a broken threat sequence  O . O O 
                                if (i_s > 1 && j_s > 1 && b[i_s - 2, j_s - 2] == state && b[i_s - 1, j_s - 1] == Gomoku.NONE)
                        {
                            score += (state == Gomoku.COMPUTER) ? BROKEN_THREAT_SEQUENCE : -BROKEN_THREAT_SEQUENCE;
                        }

                    }

                }


            }//for

            return score;
        }



        public static int GetDiagonalThreatSequences2(INode n)
        {


            var b = n as Board;
            char state = Gomoku.NONE;
            int score = 0;
            bool isHalfOpenLine;
            int j,
                i,
                s_length,// current threat sequence length
                i_s,    //threat sequence X start
                i_e,    //threat sequence X end
                j_s,    //threat sequence Y start
                j_e;    //threat sequence Y end


            j = i = 0;

            bool swich = false;
            for (int jj = BoardSize - 1; jj >= LineLenght; jj--)
            {
                if (jj == BoardSize - 1)
                {
                    if (swich) break;
                    swich = true;
                    jj = BoardSize - 1;
                }
                if (swich)
                {
                    i = jj;
                    j = 0;
                }
                else
                {
                    i = 0;
                    j = jj;
                }
                //searching for a computer state
                while (b[i, j] == Gomoku.NONE)
                {
                    i++;
                    j--;
                    if (i == BoardSize || j == -1)
                    {
                        break;
                    }
                }
                if (i == BoardSize || j == -1) continue;
                state = b[i, j]; // computer or player state
                i_s = i;  //save the sequence start x coordiante 
                j_s = j;
                //once we find it, count if we have a full threat sequence 
                s_length = 0;
                while (b[i, j] == state && s_length < LineLenght)
                {
                    s_length++;
                    i++;
                    j--;
                    if (i == BoardSize || j == -1)
                    {
                        break;
                    }
                }

                i_e = i;
                j_e = j;

                isHalfOpenLine = false;
                if (s_length == LineLenght)// we have a full threat sequence
                    score += (state == Gomoku.COMPUTER) ? FULL_THREAT_SEQUENCE : -FULL_THREAT_SEQUENCE;
                else
                {
                    if (s_length == LineLenght - 1)
                    {
                        //check if we have a half opened threat sequence    . O O O X  || X O O O . 
                        if (i_e != BoardSize - 1 && j_e > 0)
                        {
                            if (b[i_e + 1, j_e - 1] == Gomoku.NONE)
                            {
                                score += (state == Gomoku.COMPUTER) ? HALF_OPENED_THREAT_SEQUENCE : -HALF_OPENED_THREAT_SEQUENCE + 1;
                                isHalfOpenLine = true;// first limit is empty  ? O O O .

                            }
                        }
                        if (i_s > 0 && j_s < BoardSize - 1)
                        {
                            if (b[i_s - 1, j_s + 1] == Gomoku.NONE)
                            {
                                if (isHalfOpenLine)
                                {
                                    score += (state == Gomoku.COMPUTER) ? OPENED_THREAT_SEQUENCE - HALF_OPENED_THREAT_SEQUENCE : HALF_OPENED_THREAT_SEQUENCE - OPENED_THREAT_SEQUENCE;
                                }
                            }
                        }
                    }
                    else if (s_length == LineLenght - 2)
                    {

                        if ((i_e < BoardSize - 2 && i_s > 1) && (j_e > 1 && j_s < BoardSize - 2) &&
                            (b[i_e + 1, j_e - 1] == Gomoku.NONE && b[i_e + 2, j_e - 2] == Gomoku.NONE)
                             && b[i_s - 1, j_s + 1] == Gomoku.NONE && b[i_s - 2, j_s + 2] == Gomoku.NONE) // . . O O . .
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE1 : -WEACK_THREAT_SEQUENCE1;
                        }
                        else
                            if (
                                ((i_e < BoardSize - 2 && i_s > 0) && (j_e > 1 && j_s < BoardSize - 1) &&
                                (b[i_e + 1, j_e - 1] == Gomoku.NONE && b[i_e + 2, j_e - 2] == Gomoku.NONE && b[i_s - 1, j_s + 1] == Gomoku.NONE))
                                ||
                                ((i_e < BoardSize - 1 && i_s > 1) && (j_e > 0 && j_s < BoardSize - 2) &&
                                (b[i_s - 1, j_s + 1] == Gomoku.NONE && b[i_s - 2, j_s + 2] == Gomoku.NONE && b[i_e + 1, j_e - 1] == Gomoku.NONE))
                                )
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE2 : -WEACK_THREAT_SEQUENCE2;
                        }
                        else
                                if (
                                    ((i_e < BoardSize - 2) && (j_e > 1) &&
                                    (b[i_e + 1, j_e - 1] == Gomoku.NONE && b[i_e + 2, j_e - 2] == Gomoku.NONE))
                                    ||
                                    ((i_s > 1) && (j_s < BoardSize - 2) && (b[i_s - 1, j_s + 1] == Gomoku.NONE && b[i_s - 2, j_s + 2] == Gomoku.NONE))
                                    )
                        {
                            score += (state == Gomoku.COMPUTER) ? WEACK_THREAT_SEQUENCE3 : -WEACK_THREAT_SEQUENCE3;
                        }

                        else
                                    //check for a broken threat sequence  O O O . O 
                                    if (i_e < BoardSize - 2 && j_e > 1 && b[i_e + 2, j_e - 2] == state && b[i_e + 1, j_e - 1] == Gomoku.NONE)
                        {
                            score += (state == Gomoku.COMPUTER) ? BROKEN_THREAT_SEQUENCE : -BROKEN_THREAT_SEQUENCE;
                        }
                        else
                                        //check for a broken threat sequence  O . O O 
                                        if (i_s > 1 && j_s < BoardSize - 2 && b[i_s - 2, j_s + 2] == state && b[i_s - 1, j_s + 1] == Gomoku.NONE)
                        {
                            score += (state == Gomoku.COMPUTER) ? BROKEN_THREAT_SEQUENCE : -BROKEN_THREAT_SEQUENCE;
                        }

                    }

                }


            }//for

            return score;
        }

        public static int EstimateCost(INode n)
        {
            return GetVerticalThreatSequences(n) + GetHorizantalThreatSequences(n)
                 + GetDiagonalThreatSequences1(n) + GetDiagonalThreatSequences2(n);

        }


    }
}
