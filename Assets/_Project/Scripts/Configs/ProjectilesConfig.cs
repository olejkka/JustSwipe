using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Projectiles;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Projectiles",
        fileName = "Projectiles"
    )]
    public class ProjectilesConfig : ScriptableObject
    {
        public int MissTravelBoardSizeMultiplier = 2;
        public List<ProjectileDefinition> ProjectileEntries = new();


        public ProjectileDefinition GetEntryByDefinitionId(string definitionId) =>
            ProjectileEntries.FirstOrDefault(e => e.DefinitionId == definitionId);
    }
}
