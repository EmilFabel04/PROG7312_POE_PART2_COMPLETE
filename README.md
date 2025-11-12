# Municipal Services Part 3 - Service Request Status System
**Student**: ST10359034  
**Module**: PROG7312  
**Year**: 2025

## GITHUB REPO LINK:
https://github.com/EmilFabel04/PROG7312_POE_PART2_COMPLETE

## YOUTUBE VIDEO LINK:
https://youtu.be/dzOMtclS5Cw

## Overview

This Municipal Services app is an ASP.NET Core MVC application that helps citizens interact with municipal services. Part 3 focuses on the Service Request Status System, which implements advanced data structures to efficiently manage, search, and display service requests with their dependencies and priorities.

The system uses three data structures:
- **Binary Search Tree** for quick searches by request number
- **Min-Heap** for organizing requests by priority (urgent first)
- **Graph** for showing which requests depend on other requests

## Table of Contents
1. [System Requirements](#system-requirements)
2. [How to Compile and Run](#how-to-compile-and-run)
3. [Using the Service Request Status System](#using-the-service-request-status-system)
4. [Data Structures Implementation](#data-structures-implementation)
5. [Screenshots](#screenshots)
6. [Technical Architecture](#technical-architecture)
7. [Sample Data](#sample-data)
8. [Troubleshooting](#troubleshooting)
9. [References](#references)

## System Requirements

- **.NET 8.0 SDK** or higher
- **Visual Studio 2022**
- **Windows, macOS, or Linux**
- **Web browser** (Chrome, Firefox, Safari, Edge)

## How to Compile and Run

### Option 1: Download ZIP from GitHub
1. Go to: https://github.com/EmilFabel04/PROG7312_POE_PART2_COMPLETE
2. Click the green **Code** button
3. Select **Download ZIP**
4. Extract the ZIP file to your computer
5. Open the extracted folder in your terminal/command prompt

### Option 2: Clone from GitHub
```bash
git clone https://github.com/EmilFabel04/PROG7312_POE_PART2_COMPLETE.git
cd PROG7312_POE_PART2_COMPLETE
```

### Steps to Run:

#### 1. Restore Dependencies
```bash
dotnet restore
```

#### 2. Build the Solution
```bash
dotnet build
```

#### 3. Run the Application
```bash
dotnet run --project MunicipalityMvc.Web
```

#### 4. Open in Browser
Go to one of these addresses:
- **HTTPS**: `https://localhost:7001`
- **HTTP**: `http://localhost:5000`

The app will start with sample service request data ready to use.

## Using the Service Request Status System

### Main Features

#### 1. **All Service Requests** 
- Go to **Service Status -> All Requests** 
- See all service requests in a table sorted by request number
- Use the search box to find requests quickly
- Shows: Request Number, Title, Status, Priority, Category, Location

#### 2. **Priority Queue** 
- Go to **Service Status -> Priority Queue** 
- See requests ordered by priority: High first, then Medium, then Low
- Older requests with same priority come first
- Helps workers see urgent requests at the top

#### 3. **Dependencies** 
- Click **Dependencies** button next to any request
- See what other requests this one needs to wait for
- See what requests are waiting for this one
- Shows the connection between different requests

#### 4. **Search** 
- Type a request number like "SR-A2K9P3" in the search box
- Get the full details of that request quickly
- Uses the Binary Search Tree for fast searching

## Data Structures Implementation

### 1. Binary Search Tree 
**File**: `SearchTree.cs`

**What it does**: Helps find service requests quickly by their request number.

**How it works**:
- Stores requests in a tree structure
- Each request number is compared alphabetically 
- Smaller numbers go left, bigger numbers go right
- Can find any request very quickly

**Why it's better**:
- Much faster than checking every request one by one
- Keeps requests in alphabetical order automatically
- Good for the search feature and sorted lists

**Code Implementation**:
![Binary Search Tree Class Structure](docs/code_bst_class.png)
TreeNode class and SearchTree class definition 

![BST Add Method](docs/code_bst_add.png)
Add method and AddNode recursive implementation 

![BST Find Method](docs/code_bst_find.png)
Find method and FindNode recursive search 

### 2. Min-Heap (Priority Queue)
**File**: `PriorityHeap.cs`

**What it does**: Organizes service requests by how urgent they are.

**How it works**:
- High priority = 1, Medium = 2, Low = 3 (smaller number = more urgent)
- Most urgent request is always at the top
- When you add a request, it moves to the right spot automatically
- If two requests have same priority, older one comes first

**Why it's better**:
- Workers always see the most urgent request first
- Don't need to sort the whole list every time
- Adding new requests is fast

**Example**:
```
1. Water Pipe Burst (High priority)
2. Broken Streetlight (High priority)  
3. Power Outage (High priority)
4. Pothole Repair (Medium priority)
5. Garbage Collection (Medium priority)
```

**Code Implementation**:
![Min-Heap Class Structure](docs/code_heap_class.png)
PriorityHeap class definition and constructor

![Heap Add and GetNext Methods](docs/code_heap_add_getnext.png)
Add method and GetNext method implementation 

![Heap Heapify Methods](docs/code_heap_heapify.png)
HeapifyUp and HeapifyDown bubble operations 

### 3. Graph (Dependencies)
**File**: `RequestGraph.cs`

**What it does**: Shows which service requests depend on other requests.

**How it works**:
- Keeps track of connections between requests
- Some requests can't start until others are finished
- Uses DFS (Depth-First Search) to find all the requests that need to be done first
- Can also find which requests are waiting for a specific one

**Why it's useful**:
- Shows workers what order to do things in
- Prevents starting work that can't be finished yet
- Helps plan the work schedule

**Example**:
```
Traffic Light Repair
└─ needs -> Power Outage to be fixed first
   └─ needs -> Electrical Safety Check first

So the order is: Safety Check -> Power Fix -> Traffic Light
```

**DFS vs BFS**:
- **DFS**: Follows one chain all the way to the end first
- **BFS**: Looks at all direct connections first, then goes deeper
- We use DFS to show the complete chain of what needs to be done

**Code Implementation**:
![Graph Class Structure](docs/code_graph_class.png)
RequestGraph class definition and constructor 

![Graph AddRequest Method](docs/code_graph_add.png)
AddRequest method for building adjacency list 

![Graph DFS Implementation](docs/code_graph_dfs.png)
GetAllDependenciesDFS and DFS helper method 

![Graph BFS Implementation](docs/code_graph_bfs.png)
GetAllDependenciesBFS breadth-first search 

## Screenshots

### Navigation and Home Page
![Home Page with Service Status Links](docs/part3_01_home_page.png)
*Home page showing Service Request Status quick links and navigation*

### All Service Requests (Binary Search Tree)
![All Service Requests Table](docs/part3_02_all_requests.png)
*Complete list of service requests sorted by request number using BST*

![Search Functionality](docs/part3_03_search_feature.png)
*Fast search by request number demonstrating BST lookup efficiency*

![Search Results](docs/part3_04_search_results.png)
*Detailed view of found service request*

### Priority Queue (Min-Heap)
![Priority Queue View](docs/part3_05_priority_queue.png)
*Service requests ordered by priority using Min-Heap structure*

### Dependencies (Graph with DFS)
![Dependencies Page](docs/part3_06_dependencies_main.png)
*Dependencies view showing upstream and downstream relationships*

![Complex Dependency Chain](docs/part3_07_dependency_chain.png)
*Example of complex dependency chain found using DFS traversal*

## Technical Architecture

### Project Structure

![Project Structure](docs/project_structure_p3.png)

### How Everything Works Together
The `ServiceRequestStatusService` uses all three data structures:
- **LoadRequests()**: Puts the same data into all three structures
- **FindByRequestNumber()**: Uses the Binary Search Tree to find requests quickly
- **GetPriorityRequests()**: Uses the Heap to get requests in priority order
- **GetDependencies()**: Uses the Graph to find what requests depend on others

### Setup
All the services are set up in `Program.cs` so they work together properly.

## Sample Data

The app comes with 15 example service requests from Cape Town areas:

### High Priority (Urgent)
- **SR-A2K9P3**: Water Pipe Burst (Main Street, City Centre)
- **SR-B7X4M2**: Broken Streetlight (Oak Avenue, Sea Point) 
- **SR-C5N8Q7**: Power Outage (Pine Road, Camps Bay)

### Medium Priority  
- **SR-D2M9N4**: Pothole Repair (Elm Street, Observatory)
- **SR-E8P1R6**: Garbage Collection (Birch Lane, Woodstock)

### Low Priority
- **SR-F3L8K1**: Park Maintenance (Cedar Avenue, Newlands)
- **SR-G7Q4S9**: Graffiti Removal (Maple Drive, Claremont)

### Dependencies
Some requests depend on others:
- Streetlight repair waits for water pipe fix (can't do electrical work while flooding)
- Traffic light repair waits for power to be restored
- Road work waits for potholes to be fixed first

## Troubleshooting

**Application Won't Start**
- Make sure you have .NET 8.0 installed: `dotnet --version`
- Run `dotnet restore` then `dotnet build`

**Port Issues**
- Close other instances or change ports in `launchSettings.json`

**Search Not Working**
- Use exact format like "SR-A2K9P3" (case-sensitive)

**No Dependencies Showing**
- Dependencies are in the seed data, restart the app to reload

## How I Met the Requirements

### Data Structures Implementation (50 marks):
- **Binary Search Tree (BST)**: Fast O(log n) lookup of service requests by request number
- **Min-Heap (Priority Queue)**: Manages urgent requests by priority (High=1, Medium=2, Low=3)
- **Graph with DFS/BFS**: Tracks dependencies between service requests using adjacency list

### Service Request Status Features (30 marks):
- **All Requests View**: Displays sorted list using BST traversal
- **Search Functionality**: Instant lookup by request number using BST
- **Priority Queue View**: Shows requests ordered by urgency using Min-Heap
- **Dependencies Tracking**: Uses Graph DFS to show upstream/downstream dependencies
- **Unique Identifiers**: Each request has RequestNumber (user-friendly) and GUID (internal)

### Technical Implementation (20 marks):
- **MVC Architecture**: Proper separation with Controllers, Views, Services
- **Dependency Injection**: Services registered in Program.cs
- **Seeded Data**: 15 sample requests with realistic Cape Town locations
- **Efficient Operations**: All data structures provide better than O(n) performance for key operations

## References

### Part 1 & 2 References

### Microsoft Docs
- ASP.NET Core MVC: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview
- Stack<T>: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1
- Queue<T>: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1
- PriorityQueue: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2
- Dictionary: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2
- SortedDictionary: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sorteddictionary-2
- ConcurrentDictionary: https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2
- HashSet: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1
- Session State: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state
- Dependency Injection: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection

### Tutorials & Learning Resources
- W3Schools C#: https://www.w3schools.com/cs/
- W3Schools ASP.NET: https://www.w3schools.com/asp/
- GeeksforGeeks Data Structures in C#: https://www.geeksforgeeks.org/data-structures-in-c-sharp/
- TutorialsTeacher LINQ: https://www.tutorialsteacher.com/linq

### Stack Overflow Help
- Using Stack in C#: https://stackoverflow.com/questions/3188974/what-is-the-c-sharp-stack-class
- Priority Queue implementation: https://stackoverflow.com/questions/42519/how-do-you-implement-a-priority-queue-in-c
- ASP.NET Core Sessions: https://stackoverflow.com/questions/38571032/how-to-get-httpcontext-current-in-asp-net-core
- Dictionary vs Hashtable: https://stackoverflow.com/questions/301371/why-is-dictionary-preferred-over-hashtable-in-c
- ConcurrentDictionary usage: https://stackoverflow.com/questions/12611717/concurrentdictionary-addorupdate-vs-trygetvalue-tryupdate

### Other Resources
- MDN HTML Basics: https://developer.mozilla.org/en-US/docs/Web/HTML
- MDN CSS Basics: https://developer.mozilla.org/en-us/docs/Web/CSS
- OpenAI ChatGPT: helped with debugging and understanding data structures -> https://openai.com
- Prettier: https://prettier.io/ for code formatting
- Youtube : https://youtube.com/ for video tutorials and explenations

### Part 3 References

#### Binary Search Trees (BST)
- GeeksforGeeks BST Introduction: https://www.geeksforgeeks.org/binary-search-tree-data-structure/
- GeeksforGeeks BST Operations: https://www.geeksforgeeks.org/binary-search-tree-set-1-search-and-insertion/
- Programiz BST Tutorial: https://www.programiz.com/dsa/binary-search-tree
- TutorialsPoint BST in C#: https://www.tutorialspoint.com/data_structures_algorithms/binary_search_tree.htm
- CodeProject BST Implementation: https://www.codeproject.com/Articles/1158559/Binary-Search-Tree-Implementation-in-Csharp
- YouTube - Binary Search Tree Explained: https://www.youtube.com/results?search_query=binary+search+tree+tutorial+c%23

#### Min-Heap and Priority Queues
- GeeksforGeeks Heap Data Structure: https://www.geeksforgeeks.org/heap-data-structure/
- GeeksforGeeks Min Heap Implementation: https://www.geeksforgeeks.org/min-heap-in-java/
- Programiz Heap Sort Tutorial: https://www.programiz.com/dsa/heap-sort
- Microsoft Priority Queue Documentation: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2
- Stack Overflow Priority Queue C#: https://stackoverflow.com/questions/42519/how-do-you-implement-a-priority-queue-in-c
- YouTube - Min Heap Explained: https://www.youtube.com/results?search_query=min+heap+data+structure+tutorial

#### Graph Data Structure and Traversal
- GeeksforGeeks Graph Introduction: https://www.geeksforgeeks.org/graph-data-structure-and-algorithms/
- GeeksforGeeks DFS Algorithm: https://www.geeksforgeeks.org/depth-first-search-or-dfs-for-a-graph/
- GeeksforGeeks BFS Algorithm: https://www.geeksforgeeks.org/breadth-first-search-or-bfs-for-a-graph/
- Programiz Graph Traversal: https://www.programiz.com/dsa/graph-dfs
- TutorialsPoint Graph in C#: https://www.tutorialspoint.com/data_structures_algorithms/graph_data_structure.htm
- CodeProject Graph Implementation: https://www.codeproject.com/Articles/32212/Introduction-to-Graph-with-Breadth-First-Search-an
- YouTube - Graph DFS/BFS: https://www.youtube.com/results?search_query=graph+dfs+bfs+algorithm+tutorial

#### Algorithm Analysis and Big O Notation
- GeeksforGeeks Time Complexity: https://www.geeksforgeeks.org/understanding-time-complexity-simple-examples/
- Khan Academy Big O Notation: https://www.khanacademy.org/computing/computer-science/algorithms/asymptotic-notation/a/big-o-notation
- Programiz Algorithm Complexity: https://www.programiz.com/dsa/asymptotic-notations
- Stack Overflow Big O Explanation: https://stackoverflow.com/questions/487258/what-is-a-plain-english-explanation-of-big-o-notation

#### C# Resources
- Microsoft C# Collections: https://learn.microsoft.com/en-us/dotnet/standard/collections/
- C# Corner Data Structures: https://www.c-sharpcorner.com/article/data-structure-and-algorithms-using-c-sharp/
- DotNetTricks C# Data Structures: https://www.dotnettricks.com/learn/datastructures
- TutorialsTeacher C# Collections: https://www.tutorialsteacher.com/csharp/csharp-collection

#### Other Resources
- MDN Web Docs - Algorithm Basics: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Algorithms
- OpenAI ChatGPT: helped with understanding tree traversal algorithms and debugging heap operations -> https://openai.com
- Prettier: https://prettier.io/ for consistent code formatting across all data structure files
- LeetCode: https://leetcode.com/ for practicing BST, heap, and graph algorithm problems
- YouTube: https://youtube.com/ for visual explanations of how heaps maintain their properties


