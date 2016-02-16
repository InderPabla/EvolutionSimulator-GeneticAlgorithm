using UnityEngine;
using System.Collections;

public class BloopData {
    BloopDNA bloopDNA;

    public BloopData(){
        bloopDNA = new BloopDNA();
        bloopDNA.GenerateRandomBloopDNA();
    }
}
