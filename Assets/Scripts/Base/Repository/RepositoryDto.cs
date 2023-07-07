using System;
using System.Collections.Generic;
using MVC;
using UnityEngine;

[Serializable]
public class RepositoryDto<M> where M : Model
{
    [SerializeField]
    private List<M> repository = new List<M>();
    
    public RepositoryDto()
    {
    }

    public RepositoryDto(List<M> repository)
    {
        this.repository = repository;
    }

    public List<M> Repository
    {
        get
        {
            return this.repository;
        }

        set
        {
            this.repository = value;
        }
    }
}
