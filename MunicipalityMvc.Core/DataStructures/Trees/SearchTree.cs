using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.DataStructures.Trees;

public class TreeNode
{
    public string Key { get; set; }
    public ServiceRequest Data { get; set; }
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }

    public TreeNode(string key, ServiceRequest data)
    {
        this.Key = key;
        this.Data = data;
    }
}

public class SearchTree
{
    private TreeNode _root;
    
    public void Add(ServiceRequest request)
    {
        if (string.IsNullOrEmpty(request.RequestNumber))
            return;
            
        _root = AddNode(_root, request.RequestNumber, request);
    }
    
    private TreeNode AddNode(TreeNode node, string key, ServiceRequest data)
    {
        if (node == null)
            return new TreeNode(key, data);
        int compare = string.Compare(key, node.Key);
        
        if (compare < 0)
            node.Left = AddNode(node.Left, key, data);
        else if (compare > 0)
            node.Right = AddNode(node.Right, key, data);
        else
            node.Data = data;
            
        return node;
    }
    
    public ServiceRequest Find(string requestNumber)
    {
        if (string.IsNullOrEmpty(requestNumber))
            return null;
        return FindNode(_root, requestNumber);
    }
    
    private ServiceRequest FindNode(TreeNode node, string key)
    {
        if (node == null)
            return null;
            
        int compare = string.Compare(key, node.Key);
        
        if (compare == 0)
            return node.Data;
        else if (compare < 0)
            return FindNode(node.Left, key);
        else
            return FindNode(node.Right, key);
    }
    
    public List<ServiceRequest> GetAll()
    {
        var list = new List<ServiceRequest>();
        GetAllNodes(_root, list);
        return list;
    }
    
    private void GetAllNodes(TreeNode node, List<ServiceRequest> list)
    {
        if (node == null)
            return;
        GetAllNodes(node.Left, list);
        list.Add(node.Data);
        GetAllNodes(node.Right, list);
    }
    
    public bool HasRequest(string requestNumber)
    {
        return Find(requestNumber) != null;
    }
    
    public int Size()
    {
        return CountNodes(_root);
    }
    
    private int CountNodes(TreeNode node)
    {
        if (node == null)
            return 0;
            
        return 1 + CountNodes(node.Left) + CountNodes(node.Right);
    }
    
    public void Clear()
    {
        _root = null;
    }
}
