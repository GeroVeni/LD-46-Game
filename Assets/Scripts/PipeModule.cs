using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeModule : Module
{
    public Pipe[] pipes;

    bool prevState;
    const int HOLES_LIMIT = 5;

    public override void Break()
    {
        if (pipes.Length == 0) { return; }
        int index = 0;
        for (int i = 1; i < pipes.Length; ++i)
        {
            if (pipes[i].Holes < pipes[index].Holes)
            {
                index = i;
            }
        }
        pipes[index].MakeHole();
    }

    public override void ResetModule()
    {
        foreach (Pipe p in pipes)
        {
            p.ResetPipe();
        }
    }

    public override bool IsOperational()
    {
        return TotalHoles() <= HOLES_LIMIT;
    }

    int TotalHoles ()
    {
        int res = 0;
        foreach (Pipe p in pipes)
        {
            res += p.Holes;
        }
        return res;
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        prevState = true;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (prevState != IsOperational()) {
            prevState = IsOperational();
            if (prevState)
            {
                AudioManager.Instance.Stop("leak");
            } else
            {
                AudioManager.Instance.Play("leak");
            }
        }
    }
}
