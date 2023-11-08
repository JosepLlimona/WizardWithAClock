/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector2
{
    public int x;
    public int y;

    public Vector2(int _x, int _y){
        x= _x;
        y= _y;
    }
}

static class Escenari
{
    static public int[,] SpawnEscenari(Vector2 size, int quadratsPerGenerar, float dpc, Vector2 midaParets){
        int[,] MapaEscenari;
        MapaEscenari = CrearQuadratsEscenari(size.x, size.y, quadratsPerGenerar, dpc);
        int nSales = TrobarSales(MapaEscenari, size.x, size.y);
        
        if (nSales >1){
            MapaEscenari = CrearParetsEscenari(MapaEscenari, size.x, size.y, nSales, midaParets);
        }
        else{
            return SpawnEscenari(size, quadratsPerGenerarm, dpc, midaParets);
                return MapaEscenari;
            }
        }
    }

    static int[,] CrearQuadratsEscenari(int size_x, size_y, int nQuadrats, float dpc){
        int[,] newMapa = new int[size_x,size_y];

        for (int i = 0; i < nQuadrats; i++){
            int midaSala_X = Random.Range(10, Mathf.RoundToInt(size_x/dpc));
            int midaSala_Y = Random.Range(10, Mathf.RoundToInt(size_y/dpc));
            int salaPos_X = Random.Range (2,size_x-midaSala_X - 1 );
            int salaPos_Y = Random.Range (2,size_y-midaSala_Y - 1 );

            for(int j = salaPos_X; j < salaPos_X + roomSizeX; j++){
                for(int k = salaPos_Y; k < midaSala_Y + midaSala_X; k++){
                    newMapa[j,k] = 1;
                }
            }
        }
        return newMapa;
    }

    static int TrobarSales(int[,] MapaEscenari, size_x, size_y){
        List<Vector2> possiblesCoord = new List<Vector2>();
        int[,] mapaModificat = new int[size_x, size_y];
        int nSalesTrobades = 0;

        System.Array.Copy(MapaEscenari, mapaModificat, size_x*size_y);

        for(int i = 0; i < size_x; i++){
            for(int j = 0; j < size_y; j++){

                if(mapaModificat[i,j] == 1){
                    possiblesCoord.Add(new Vector2(i,j));

                    while(possiblesCoord.Count > 0){
                        int x = (int)possiblesCoord[0].x;
                        int y = (int)possiblesCoord[0].y;
                        possiblesCoord.RemoveAt (0);
                        MapaEscenari[x,y] = nSalesTrobades +1;

                        for(int aux_x = x-1; aux_x <= x+1; aux_x++){
                            for(int aux_y = y-1; aux_y <= y+1; aux_y ++){
                                if(mapaModificat[aux_x,aux_y] == 1){
                                    possiblesCoord.Add(new Vector2(aux_x,aux_y));
                                }
                            }
                        }
                    }
                    nSalesTrobades++;
                }
            }
        }
        return nSalesTrobades; 
    }
    
    static private int[,] CrearParetsEscenari(int[,] MapaEscenari, int size_x, int size_y, int nSales, Vector2 midaParets){
        int x1, x2, y1, y2;

        for(int passadisNum = 1; passadisNum <= nSales; passadisNum++){
            int nSalesProb = 0;
            int nSalesProbMax = 3000;

            x1 = Random.Range (1, size_x-1);
            y1 = Random.Range (1, size_y-1);

            while(MapaEscenari[x1,z1] != passadisNum && nSalesProb < nSalesProbMax){
                x1 = Random.Range (1, size_x-1);
                y1 = Random.Range (1, size_y-1);
                nSalesProb++;
            }
            nSalesProb = 0;

            x2 = Random.Range (1, size_x-1);
            y2 = Random.Range (1, size_y-1);

            while((MapaEscenari[x2,y2] == 0 || MapaEscenari[x2,y2] == passadisNum) && nSalesProb < nSalesProbMax){
                x2 = Random.Range (1, size_x-1);
                y2 = Random.Range (1, size_y-1);
                nSalesProb++;
            }
            int dife_x = x2-x1;
            int dife_y = y2-y1;

            int xDirection = 1;
            int yDirection = 1;

            if(dife_x != 0){
                xDirection = dife_x/Mathf.Abs(dife_x);
            }
            else{
                xDirection = 0;
            }

            if(dife_y != 0){
                yDirection = dife_y/Mathf.Abs(dife_y);
            }
            else{
                yDirection = 0;
            }

            int paretWidth = Random.Range (midaParets.x,midaParets.y);
            int paretHeight = Random.Range (midaParets.x,midaParets.y);

            for(int i = x1; i != x1 + xDirection*paretWidth; i += xDirection){
                for(int j = y1; j != y2; j += yDirection){
                    if(i >= 0 && i < size_x && j >= 0 && j < +size_y){
                        if(MapaEscenari[i,j] == 0){
                            MapaEscenari[i,j] = -1;
                        }
                    }
                }
            }

            for(int i = x1; i != x2; i += xDirection){
                for(int j = y2; j != y2 + yDirection*paretHeight; j += yDirection){
                    if(i >= 0 && i < size_x && j >= 0 && j < +size_y){
                        if(MapaEscenari[i,j] == 0){
                            MapaEscenari[i,j] = -1;
                        }
                    }
                }
            }
        }
        return MapaEscenari;
    }



    
}*/
