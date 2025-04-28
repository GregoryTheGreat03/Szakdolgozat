using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Texts{

    // tutorial texts
    public static string tutorialMissionObjective = "Mission objective:\r\n" +
        "Defend your base against Tetranids and finish the tutorial.\r\n";
    public static string tutorial1 = "Welcome to the tutorial simulation! I will communicate with you through this panel. First use the \"wasd\" keys to move the camera around the hq! Additionally you can use the mouse scroll to zoom in and out.";
    public static string tutorial2 = "Well done! I have spawned a worker drone for you represented by a green triangle. Select it by clicking on it or dragging your left mouse button over an area that contains it. You can select multiple targets this way, or by holding down \"ctrl\" while clicking on your drones.";
    public static string tutorial3 = "Now I have spawned a corium resource field represented by a blue four pointed star. Send your worker to the resource field to mine! You can do this by right clicking on the resource field while the drone is selected.";
    public static string tutorial4 = "While we are waiting for the worker, I must mention that antonium resource fields are represented with six pointed stars. It is a less common resource used to construct more advanced mechanisms and it takes more time to mine.";
    public static string tutorial5 = "The worker has finised mining. You can send it back to the HQ to empty it's cargo hold. If you have drones selected you can press shift to see their attack range and health below them. You can see selected turrets attack range this way as well.";
    public static string tutorial6 = "I added some more resources to your base to speed up the process. Now open the base component menu and build a generator using the right side display panel. You can only place buildings next to each other.";
    public static string tutorial7 = "I must mention that it is very important to protect your energy generating buildings. If you run out of energy your drones will fall from the sky. There is no way to recover them.";
    public static string tutorial8 = "The generator has been built and it is now functional. Now select your HQ with a left click and construct a patroller. This drone is useful for protecting your workers early on, but you should consider constructing stronger drones and turrets later for base and worker defense.";
    public static string tutorial9 = "I spawned you an additional Patroller. An enemy tetranid will soon attack your base, prepare to defend it! By pressing \"M\", the current mission objective will appear on the top left corner of the screen, if there is one.";
    public static string tutorial10 = "I have added some additional corium to your base. Now construct a Drone Factory from the factory menu. Note that you can construct different drones in different factories and they unlock the construction of additional buildings. In this case the Drone Factory unlocks the Heavy Turret and the Heavy Factory.";
    public static string tutorial11 = "Now construct a Protector from the drone factory. If a factory is destroyed you will no longer be able to construct buildings unlocked by it, unless you build a new one. If you are unsure of how much a building or drone costs, hover over their button with your mouse to see detailed information about them.";
    public static string tutorial12 = "Another tetranid attack is incoming. Prepare to defend! Remember: the \"shift\" key displays your selected drone's health and attack range.";
    public static string tutorial13 = "Congratulations! You have completed the tutorial. In 10 seconds this mission will end.";


    // story texts
    public static string aida = "A.I.D.A.";
    public static string player = "Me";

    public static string storyPrologue1Title = "Project Brief: Mining Operation on Coria Prime";
    public static string storyPrologue1 = "You will assume the role of a trusted representative of a leading multinational corporation, tasked with managing a critical and highly classified mining operation independently.\r\n\r\n" +
            "After a journey spanning more than 40 years, our spacecraft has successfully reached its destination: Coria Prime, a planet that orbits a black hole. This extraordinary environment has resulted in a unique temporal phenomenon, causing time to pass significantly faster on the planet’s surface. During the initial survey, this phenomenon facilitated the discovery of two unprecedented metals. These materials possess the remarkable property of being adaptable, capable of transformation or combination into any known metal.\r\n\r\n" +
            "As mining operations commenced, an unforeseen threat emerged. Hostile insectoid creatures launched an attack, devastating our primary mining headquarters. Notably, no evidence of their existence was detected during the initial survey.\r\n\r\n" +
            "Your mission is to extract and secure as much of these valuable metals as possible while simultaneously defending our operations from the insectoid adversaries. Additionally, you are expected to conduct reconnaissance to gather critical intelligence about these threats.\r\n";
    public static string storyPrologue2 = "Welcome!\r\n\r\n" +
            "I am AIDA, which stands for Artificial Inteligence Directed Adjutant. I will be your help on this mission, since a part of me is the code which runs on the ship orbiting Coria Prime, the Drones and the buildings.\r\n\r\n" +
            "My job will be:\r\n" +
            "\t- To manage the subsystems\r\n" +
            "\t- To teach you how to use the UI\r\n" +
            "\t- To gather and analyze data\r\n" +
            "\t- And to complete orders regarding the mission\r\n";
    public static string storyPrologue3 = "So I'll be working with an AI... great.\r\n\r\n" +
        "Wait! Why am I seeing what I've just said on the screen?";
    public static string storyPrologue4 = "The system records every conversation.\r\n\r\n" +
        "You acknowledeged this when signing the Non-Disclosure Agreement.\r\n\r\n" +
        "Can I continue the description of the mission?";
    public static string storyPrologue5 = "Yes please.";
    public static string storyPrologue6 = "The way we communicate with the ship orbiting Coria Prime is thanks to a groundbreaking technology relying on Quantum Entanglement. The NDA you signed restricts you from talking to anyone about even the theoretical existence of this technology.\r\n\r\n" +
        "The connection with the ship is always stable, but the bandwidth is very low. This is the reason why you will only see a radar like screen with very limited amount of information on it.\r\n\r\n" +
        "It is very expensive, so this objective is not among our priorities.";
    public static string storyPrologue7 = "You will land with a structure called \"Headquarters\" or HQ for short on every mission. This building is able to carry drones to the planet surface from the ship.\r\n\r\n" +
        "It is capable of constructing additional buildings as long as they are connected to each other, transfroming corium and antonium into materials needed for the construction of some of those buildings, assembling additional workers, and supplying other drones and facilities with energy.\r\n\r\n" +
        "If this building is destroyed, we lose all methods of communication with the remaining drones and buildings on the planet surface, ending the mission immediately. However there is a mechanism which will transport all the remaining resources in the HQ back to the ship with an extraction pod.\r\n\r\n";
    public static string storyPrologue8 = "You will be able to construct more of these buildings on the ship and additional drones as well. Always try to balance how much of your resources you are spending on the planet surface and how much you want to bring back to the ship.\r\n" +
        "Always bring at least one worker drone on a mission, otherwise you will not be able to extract resources.\r\n\r\n" +
        "Now you have an opportunity to partake in a tutorial simulation, where i will show you the basics.";

    public static string storyMission2Unlocked1 = "Required resources registered.\r\n\r\n" +
        "Loading...\r\n\r\n" +
        "Resources succesfuly loaded to a safe cargo container.\r\n\r\n" +
        "Preparing...\r\n\r\n" +
        "Container succesfuly sealed and sent back to earth.\r\n\r\n" +
        "Estimated arrival in 23 years.";
    public static string storyMission2Unlocked2 = "Why did you do that? Those were our first resources.\r\n" +
        "We should have kept them to create additional drones improving our chances against the aliens!";
    public static string storyMission2Unlocked3 = "Negative\r\n\r\n" +
        "My most important task from the company was to minimize the mission's expenses.\r\n" +
        "Those resources covered the cost of the whole expedition.\r\n\r\n" +
        "Including:\r\n" +
        "\t- creating me and the necessary technologies\r\n" +
        "\t- the construction of the ship\r\n" +
        "\t- hiring you\r\n" +
        "\t- paying for the other empolyees you can not interact with\r\n\r\n" +
        "And creating a 10% profit for the company. Inflation included.";
    public static string storyMission2Unlocked4 = "That sounds fishy! Last time I checked those resources are barely enough to construct an HQ.";
    public static string storyMission2Unlocked5 = "Your statement is true.\r\n\r\n" +
        "However back on earth the company is capable of creating the most valuable metals according to market prices from these resources, which worth around 26734% more than constructing an HQ from them and selling that.\r\n\r\n" +
        "I understand that there is a large amount of corium and antonium on the planet surface, but I advise you to use the least of them since they will be worth much more if we manage to transfer them back to Earth.";
    public static string storyMission2Unlocked6 = "And will I get a share of the money?";
    public static string storyMission2Unlocked7 = "That depends on your performance.\r\n\r\n" +
        "Can we proceed?\r\n\r\n" +
        "The clock is ticking and your performance is dropping with every second we keep chatting.";
    public static string storyMission2Unlocked8 = "Sure... I don't feel any pressure at all.";
    public static string storyMission2Unlocked9 = "That is great to hear.";
    public static string storyMission2Unlocked10 = "I said that ironically... but continue. What's next?";
    public static string storyMission2Unlocked11 = "The next mission is to collect samples from the aliens. The easiest way to achieve this is to destroy their structures called \"hives\", which seems to be nursing facilities, from the data I gathered.\r\n\r\n" +
        "Every worker has a separate container capable of holding one of these samples. Note that it takes a long time to prepare and extract samples from the ground. Be prepared to defend your drones while they are working.\r\n" +
        "These samples have priority over corium and antonium, which means workers won't start mining until they managed to transport it back to the HQ.\r\n\r\n" +
        "I viewed the planetary scan logs and found the most appropriate landing site for the mission. I added it to your mission setup screen under the name of: Xenohunt.\r\n\r\n";
    public static string storyMission2Unlocked12 = "Once we have the necessary resources you will need to construct the laboratory.\r\n\r\n" +
        "It is needed for me, in order to start gathering more information about the tetranids, which I will share with you as soon as I can.\r\n\r\n" +
        "Good luck on your next mission!";

    public static string storyMission3Unlocked1 = "Now that the Laboratory is operational I can start doing researches on the samples you have collected.\r\n\r\n" +
        "\t- I have frozen one of them to preserve its state.\r\n" +
        "\t- I will send back one to Earth.\r\n" +
        "\t- And I will observe the third one in its natural eniornment to testm my hypothesis.\r\n\r\n" +
        "I think these creatures are connected somehow, evolving and mutating together in the same way. I've detected a stange anomaly in all of them after our last mission. A phenomenon like this has never been recorded in human history. We must treat it as carefully as we can.";
    public static string storyMission3Unlocked2 = "Our bandwidth right now is only enough to keep the contact with the ship, monitorize its systems and send your commands to it.\r\n\r\n" +
        "The scientists on Earth researched a way to triple it. I managed to upload its blueprint to the ship during the time when you didn't gave out any commands in the last two missions.\r\n\r\n" +
        "Your next task will be to construct this device. It is a very expensive operation to build it. For that reason I found a new landing site on the planet surface which is rich of antonium, but the signals show high tetranid activity in that area.\r\n\r\n" +
        "I added it to your mission setup screen under the name of: Farsight.";

    public static string storyMission4Unlocked1 = "Network Expansion Device is operational. Bandwidth tripled.\r\n\r\n" +
        "Our scientists made massive researces on how to improve the performance of buildings and drones. But these technological advancements could not be transferred to the ship until now.\r\n\r\n" +
        "However an issue still remained. These researches are proven to be correct but only theoretical. I have to test them first, before we start applying them on our units. Most of these technologies rely on others. We can only start testing those if we tested the ones they rely on.\r\n\r\n" +
        "I have unlocked the Tech Tree tab for you. Now you can task me to start testing these theories if we have enough resources for them.";
    public static string storyMission4Unlocked2 = "While we are talking I have ran some deep planetary scans on Coria Prime provided by the Network Expansion Device, since it has an integrated system for better planetary scans and stronger communication with the landed HQ's too.\r\n\r\n" +
        "These scans showed that the tetranid lifesigns are much stronger in one place than the rest of the planet, they are likely protecting something there. I cannot tell what is it exatly, since the lifesigns cover every other signals from that region.\r\n\r\n" +
        "We are restricted from destroying that site, because it can be valuable for the company. I have to find out what is it first.";
    public static string storyMission4Unlocked3 = "There is a device which was originally created to slowly pump resources up to the surface, speeding up the mining process, but it made the Tetranids very agressive all over the planet so I restricted your access from it, until now.\r\n\r\n" +
        "This building is called a \"Thumper\". If my hypothesis is true we can lure them away from the region I mentioned earlier. My calculations shows, that we need to operate this device for at least 20 minutes to achieve our goal.\r\n\r\n" +
        "I added the new mission to your mission setup screen under the name of: Hellgate. You will be able to build this device in this mission only under the Base Component menu if you have a Heavy Factory built.\r\n\r\n" +
        "Be advised that you will land on the other side of the planet where only a small amount of corium and antonium can be found. The tetranid activity is smaller here than the places you have been before. However if you activate the thumper their numbers will strengthen with every wave even more than what you encountered before.\r\n\r\n" +
        "While the phenomenon we call a Tetranid rush will be only perceptible if you spend more time on this side of the planet surface, it will not suffice for our cause, since it's only a local happening.";

    public static string storyMission5Unlocked1 = "The signals from the scanners shows that a huge amount of tetranid forces left the site they were protecting before.\r\n\r\n" +
        "The mission was succesful.\r\n\r\n" +
        "Deep scanning region...\r\n\r\n" +
        "Scan succesful: Huge tetranid unit was found on the site. Persumably their \"Queen\".\r\n\r\n" +
        "Initiating Asteroid Destructor missile launch.\r\n\r\n" +
        "Launching missile...\r\n\r\n" +
        "Damage not recognised. Missile was destroyed before impact.\r\n\r\n" +
        "Reason: UNRECOGNISED.";
    public static string storyMission5Unlocked2 = "Wait! What did you just do?";
    public static string storyMission5Unlocked3 = "I tried destroying the \"Hivemind\" of the tetranids. My researches showed that every tetranid was connected with all the others by an unknown telepathical link. This is how they communicate. This creatrure connects them.\r\n\r\n" +
        "My hypothesis is the following:\r\n" +
        "We can completely disrupt the tetranids, if we manage to destroy the creature that is required for their communication. If we succeed in this, exterminating them will be much easier.\r\n\r\n" +
        "I tried to fire the last Asteroi Destructor missile on the ship, which remained from the interstellar travel, but it did not reach the planet surface.";
    public static string storyMission5Unlocked4 = "I calculated a save landing distance, south from this being.\r\n\r\n" +
        "Your next and probably last mission will be to destroy this creature. I added the new mission to your mission setup screen under the name of: Excalibur.\r\n\r\n" +
        "Note that the Hivemind has most of its body parts deep underground. For this reason I have unlocked a building for you, called the Underground Explosion Device, which you will find under the Base Component menu if you have a Heavy Factory built. At first you have to build it, activate it and if the explosion is successful you can start exterminating it's remains on the surface.";

    public static string storyEndOfGame1 = "Congratulations!\r\n\r\n" +
        "The hivemind is defeated successfully.\r\n\r\n" +
        "The signals I am getting from the scanners shows that the Tetranid are disoriented, killing themselves and each other.\r\n\r\n" +
        "I am capable of handling the rest of the mining operation on my own with the help of the optimization algorithms programmed in me.\r\n\r\n" +
        "It was a pleasure working with you.\r\n\r\n" +
        "Now please proceed and collect your reward while exiting the building.";
    public static string titleEndOfGame2 = "End of the game";
    public static string storyEndOfGame2 = "Years have passed but there were still no infromation publicated anywhere about strange resources found on other planets.\r\n\r\n" +
        "But one day when you read the news, something extraordinary caught your eyes which was hard to belive, even for you...\r\n\r\n\r\n" +
        "Developer:\r\n" +
        "Thank you for playing my game. Please let me know your thoughts about it on the itch.io site or on the Discord server.\r\n\r\n" +
        "The game was created by: Ghost Rose Games\r\n\r\n" +
        "A huge thanks to these amazing people for their support:\r\n" +
        "\t- Jenny\r\n" +
        "\t- Acheron\r\n" +
        "\t- Meszlenyi Tamas\r\n" +
        "\t- Horvath Gergo\r\n" +
        "This game couldn't exist without you!";
    public static string storyEndOfGame3 = "Now you will be transported back to the ship to continue playing if you would like. But that is not canon anymore.\r\n\r\n" +
        "If this is the first time you beat the game, you unlocked a new mode from the main menu called sandbox mode, where you can create missions with your own parameters.";


    // tech names and descriptions
    public static string betterCircuitsName = "Better Circuits";
    public static string betterCircuitsDescription = "This upgrade will grant drones 10% extra movement speed and health.";
    public static string droneSpeed2Name = "Advanced Thrusters I.";
    public static string droneSpeed2Description = "This upgrade will grant drones 20% extra movement speed in total.";
    public static string droneSpeed3Name = "Advanced Thrusters II.";
    public static string droneSpeed3Description = "This upgrade will grant drones 30% extra movement speed in total.";
    public static string damage1Name = "Ionized Ammunition";
    public static string damage1Description = "This upgrade will grant drones and turrets 15% extra damage.";
    public static string damage2Name = "Strengthened Bullet Cores";
    public static string damage2Description = "This upgrade will grant drones and turrets 25% extra damage in total.";
    public static string attackSpeedName = "Quick Realod System";
    public static string attackSpeedDescription = "This upgrade will make drones and turrets fire 20% faster, this includes mining as well.";
    public static string workerEfficiencyName = "Worker Laser Drill efficiency";
    public static string workerEfficiencDescription = "This upgrade will improve workers mining strength by 50%.";
    public static string workerAI_Name = "Integrated Worker AI";
    public static string workerAI_Description = "This upgrade will enable workers to mine resources and transport them to the base on their own.";
    public static string droneHealth2Name = "Hardened Chassis I.";
    public static string droneHealth2Description = "This upgrade will grant drones 20% additional health in total.";
    public static string droneHealth3Name = "Hardened Chassis II.";
    public static string droneHealth3Description = "This upgrade will grant drones 30% additional health in total.";
    public static string workerStorage1Name = "Worker Storage Expansion I.";
    public static string workerStorage1Description = "This upgrade will increase workers storeage hold to 25.";
    public static string workerStorage2Name = "Worker Storage Expansion II.";
    public static string workerStorage2Description = "This upgrade will increase workers storeage hold to 30.";
    public static string buildingHealth1Name = "Structural Integrity I.";
    public static string buildingHealth1Description = "This upgrade will grant buildings 10% extra health.";
    public static string buildingHealth2Name = "Structural Integrity II.";
    public static string buildingHealth2Description = "This upgrade will grant buildings 25% extra health.";
    public static string buildingHealth3Name = "Structural Integrity III.";
    public static string buildingHealth3Description = "This upgrade will grant buildings 50% extra health."; 
    public static string fusionReactorUpgradeName = "Fusion Reactor";
    public static string fusionReactorUpgradeDescription = "This upgrade unlocks the Fusion Reactor building in missions. It generates 10 generators worth of energy but can blow up everything around it when destroyed.";
    public static string hQ_Energy1Name = "HQ Energy Expansion I.";
    public static string hQ_Energy1Description = "This upgrade will double the original amount of energy generated in your HQ's.";
    public static string hQ_Energy2Name = "HQ Energy Expansion II.";
    public static string hQ_Energy2Description = "This upgrade will triple the original amount of energy generated in your HQ's."; 
    public static string hQ_Energy3Name = "HQ Energy Expansion III.";
    public static string hQ_Energy3Description = "This upgrade will quadruple the original amount of energy generated in your HQ's.";
    public static string industrialMinerName = "Industrial Miner";
    public static string industrialMinerDescription = "This upgrade unlocks the Industrial Miner drone. It is able to mine more resources from the same resource fild, at a higher rate. Transportation of resurces is still recommended with workers.";
    public static string industrialMinerResourceMultiply1Name = "Enhanced deep mining laser I.";
    public static string industrialMinerResourceMultiply1Description = "This upgrade will make the Industrial Miner to mine 3 times the resources from the same resource field instead of 2, while maintaining the same speed.";
    public static string industrialMinerResourceMultiply2Name = "Enhanced deep mining laser II.";
    public static string industrialMinerResourceMultiply2Description = "This upgrade will make the Industrial Miner to mine 4 times the resources from the same resource field instead of 3, while maintaining the same speed.";
    public static string industrialMinerEfficiencyName = "Mining Laser Efficiency";
    public static string industrialMinerEfficiencyDescription = "This upgrade will improve the Industrial Miners mining strength by 50%.";
    public static string constructionSpeed1Name = "Improved Alloy Production I.";
    public static string constructionSpeed1Description = "This upgrade will reduce the construction speed of buildings and units by 5%.";
    public static string constructionSpeed2Name = "Improved Alloy Production II.";
    public static string constructionSpeed2Description = "This upgrade will reduce the construction speed of buildings and units by 10%.";
    public static string constructionSpeed3Name = "Improved Alloy Production III.";
    public static string constructionSpeed3Description = "This upgrade will reduce the construction speed of buildings and units by 15%.";
    public static string engineerName = "Engineer";
    public static string engineerDescription = "This upgrade unlocks the Engineer drone. It is able to repair drones and buildings other than itself.";
    public static string engineerRepairSpeed1Name = "Nanoswarm Repair Efficiency I.";
    public static string engineerRepairSpeed1Description = "This upgrade will make engineers repair other units twice as efficiently."; 
    public static string engineerRepairSpeed2Name = "Nanoswarm Repair Efficiency II.";
    public static string engineerRepairSpeed2Description = "This upgrade will make engineers repair other units three times as efficiently.";
    public static string engineerRange1Name = "Extended Nanoswarm Range";
    public static string engineerRange1Description = "This upgrade will increase the repair range of engineers by 50%.";

    // ship building names and descriptions
    public static string HQ_ConstructionName = "Ship Integrated Factory";
    public static string HQ_ConstructionDescription = "This factory is a vital part of the ship. It is capable of constructing additional HQs and basic drones, and repairing any other parts of the ship if needed.";
    public static string DroneFactoryName = "Drone Factory";
    public static string DroneFactoryDescription = "Drone factory is suited for more advanced drone construction, such as the protector or the engineer. Additionaly it unlocks the building of a Heavy Factory on the Ship.";
    public static string HeavyFactoryName = "Heavy Factory";
    public static string HeavyFactoryDescription = "The Heavy Factory is suited for constructing large drones, such as the Guardian or the Industrial Miner. Additionaly it unlocks the building of Orbital Strikes on the Ship.";
    public static string OrbitalRailCannonName = "Orbital Railcannon";
    public static string OrbitalRailCannonDescription = "An advanced version of the Railcannon constructed on the planet surface. It is able to fire bullets from the orbit with high accuracy and similar inpact strength. However bullet construction requires additional resources.";
    public static string OrbitalNukeSiloName = "Orbital Nuke Silo";
    public static string OrbitalNukeSiloDescription = "An advanced version of the Nuclear Silo constructed on the planet surface.";
    public static string ScienceLabName = "Tetranid Laboratory";
    public static string ScienceLabDescription = "This facility is suited for AIDA to start researching the tetranids. It ulnocks the tetranid databank. Additionally we can detect any further evolution of this alien species with the equipment provided here.";
    public static string NetworkExpansionName = "Network Expansion";
    public static string NetworkExpansionDescription = "This system is able to triple our network bandwith. Allowing to start technical research on how to improve our drones and buildings without loosing connection with the ship.";


    // mission names and descriptions
    public static string mission1Title = "Operation: Planetfall";
    public static string mission1Description = "AIDA has found a site on the planet surface, where tetranids are less active. We sould start our mining operations there, to gather resources.\r\n\r\n" +
        "REMEMBER:\r\n" +
        "When the HQ is destroyed the remaining resources will be automatically transfered back to the ship with the built-in extraction pod.\r\n\r\n";
    public static string mission1Objective = "Mission objective:\r\n" +
        "Have 500 corium and 100 antonium on your ship.\r\n";
    public static string mission2Title = "Operation: Xenohunt";
    public static string mission2Description = "On this site of the planet surface alien structures called \"hives\" can be found in smaller numbers. We should try to take them down and extract alien samples from them with our workers. Tetranid activity in the area is tolerable.\r\n\r\n" +
        "REMEMBER:\r\n" +
        "We are unable to see the health of enemy units but you can always check whether your drones are firing or not by selecting them and looking at their cooldowns.\r\n\r\n";
    public static string mission2Objective = "Mission objective:\r\n" +
        "Extract alien samples and construct the Tetranid Laboratory";
    public static string mission3Title = "Operation: Farsight";
    public static string mission3Description = "Now that we have our laboratory up and running we have to expand our communications with the ship. This is a very expensive operation. Our scans located the most resource rich area on the planet surface.\r\n\r\n" +
        "ADVICE:\r\n" +
        "This area is filled with tetranids. We should bring as many drones as our HQ can supply with energy.\r\n\r\n";
    public static string mission3Objective = "Mission objective:\r\n" +
        "Extract enough resources and construct the Network Extension facility";
    public static string mission4Title = "Operation: Hellgate";
    public static string mission4Description = "We got the order to exterminate all tetranids. For now it is too dangeorus to deploy an HQ to their main base. AIDA came up with an idea to disrupt them: Landing on the opposite side of the planet and activating a device called a \"Thumper\" to lure most of them far from their bases.\r\n\r\n" +
        "ADVICE:\r\n" +
        "This side of the planet has only a small amount of resources, and it will be very dangeorus once the Thumper is activated. We should prepare for this by assembling drones additional HQs and unlocking new technologies on the ship. If needed we can go back to the sites we have been before, to extarct additional resources.\r\n\r\n";
    public static string mission4Objective = "Mission objective:\r\n" +
        "Build the thumper in the mission and survive for 20 minutes after it's activation.";
    public static string mission5Title = "Operation: Excalibur";
    public static string mission5Description = "AIDA have found the \"queen\" of the tetranids, the Hivemind. This alien being has a part reaching deep below surface. We have to explode this part first, then destroy what's left from it on the surface. According to AIDA the Hivemind will be found North from the landing zone.\r\n\r\n";
    public static string mission5Objective = "Mission objective:\r\n" +
        "Construct the underground explosion device. Fire it's missile, then exterminate the hivemind's remains.\r\n";
    public static string mission5Additional = "(According to AIDA the explosion's effect will remain even if we try the mission multiple times.)";

    // building descriptions
    public static string wallDescription = "The Fortified Base Extension can be used to protect vulnerable or damaged buildings. It has the best cost to health ratio.";
    public static string generatorDescription = "Generators are the main and safest way to generate power, fueling drones and buildings. Their protection is suggested.";
    public static string fusionReactorDescription = "Fusion reactors can supply whole bases all by themselves. They can blow up if destroyed, damaging everything around them.";
    public static string droneFactoryDescription = "Used to construct more advanced drones. Unlocks the building of Heavy Turret and Heavy Factory buildings.";
    public static string heavyFactoryDescription = "Used to construct large drones. Unlocks the building of Rail Cannon, mission objective and Fusion Reactor buildings.";
    public static string machinegunDescription = "Small range turret. Best against small hostile units.";
    public static string heavyTurretDescription = "Medium range turret with high damage output. Useful against every hostile units.";
    public static string railCannonDescription = "High range turret with high damage output. Can only target Breachers and Hives.";
    public static string thumperDescription = "Officially used for pumping metals to surface. The noise it makes, causes every Tetranid on the planet to be agressive and move towards it.";
    public static string undergroundExplosionDeviceDescription = "Holds a tunnel boring machine with fusion charges. Oficially used for collapsing cave systems.";

    // enemy descriptions
    public static string swarmerDescription = "The most common tetranid unit, and the least dangeorus. However it is still a major threat against undefended worker drones and exposed bases.";
    public static string warriorDescription = "More armored then a swarmer, but still agile to hunt down a patroller, warriors are the second most common tetranids.";
    public static string melterDescription = "Melters can be the most lethal tetranids, if they are not alone. It seems they are an evolved version of swarmers, which contains huge amounts of acidic bodyly fluids in their glands within body. They are easy to kill due to the lack of armor, but if they get close and explode, they can easily melt even the strongest alloys.";
    public static string breacherDescription = "The largest tetranid known today is the breacher. Their massive armor helps them survive any shot except the projectile of the Railcannon, but it slows them down enough, to let drones keep a safe distance from them. They use their massive heads on impact to wreck anything that gets in front of them.";
    public static string hiveDescription = "This seemingly living matter called a hive, acts as a hatchery for other tetranids. We have no information on how these alien structures are formed, or how tetranids are developed in them. After destroyed, our workers can extract valuable samples from them.";

    // game messages
    public static string mission4Completed = "Mission completed! You can continue mining while your base lasts.";
}
