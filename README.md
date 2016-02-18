# Evolution Simulator Of Multinode Creatures
## The Main Idea
This is an evolution simulator built using Unity3D using language C#. This simulator creates creatures with nodes and muscles which are tested to survive in a world where traveling the furthest right is considered to have the highest fitness. Each creatures has 3 to 10 nodes. Each node can be connected to many other nodes using muscles. Each node has 4 properties: starting X, staring Y, friction factor and bouncy factor. Each muscle also has 4 properties: minimum distance to be collapsed, maximum distance to be expanded, muscle stretch frequency and muscle stretch dampening. 

## Reproduction 
Reproduction in this world is asexual. After all the creatures are tested, half of the slowest creatures are removed from the genepool, and the other half go through reproduction. Each creature cerates 2 perfect copies of itself. The first copy goes through immediate mutation which will change some property of the nodes or the muscles. The second copy has a 5% chance of going through mutation, this will sever to preserve the best creature’s DNA from each generation. 


## GIF
![](http://imgur.com/KaOqHfH.gif)
![](http://imgur.com/Qd7RTK9.gif)

## Pictures
### Creature 1 - Generation 63
#### Ability to run 41 meters in 15 seconds
![Alt text](https://github.com/InderPabla/EvolutionSimulator-GeneticAlgorithm/blob/master/Images/creature1.png "")
![Alt text](https://github.com/InderPabla/EvolutionSimulator-GeneticAlgorithm/blob/master/Images/creature1muscles.png "")

### Creature 2 - Generation 51
#### Ability to run 54 meters in 15 seconds
![Alt text](https://github.com/InderPabla/EvolutionSimulator-GeneticAlgorithm/blob/master/Images/creature2.png "")
![Alt text](https://github.com/InderPabla/EvolutionSimulator-GeneticAlgorithm/blob/master/Images/creature2muscles.png "")

### Creature 3 - Generation 105
#### Ability to run 72 meters in 15 seconds
![Alt text](https://github.com/InderPabla/EvolutionSimulator-GeneticAlgorithm/blob/master/Images/creature3.png "")
![Alt text](https://github.com/InderPabla/EvolutionSimulator-GeneticAlgorithm/blob/master/Images/creature3muscles.png "")

### Creature 4 - Generation 105 - SECOND FASTEST CREATURE
#### Ability to run 81 meters in 15 seconds
![Alt text](https://github.com/InderPabla/EvolutionSimulator-GeneticAlgorithm/blob/master/Images/creature4muscles.png "")

### Creature 5 - Generation 120 - FASTEST CREATURE - Usain Bolt (well...not really)
#### Ability to run 105 meters in 15 seconds
![Alt text](https://github.com/InderPabla/EvolutionSimulator-GeneticAlgorithm/blob/master/Images/creature5.png "")
![Alt text](https://github.com/InderPabla/EvolutionSimulator-GeneticAlgorithm/blob/master/Images/creature5muscles.png "")
