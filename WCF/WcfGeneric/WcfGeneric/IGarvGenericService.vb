' NOTE: You can use the "Rename" command on the context menu to change the interface name "IService1" in both code and config file together.
<ServiceContract()>
Public Interface IGarvGenericService
    <OperationContract()>
    Function InsertEmployee(ByVal employee As Employee) As Integer
End Interface