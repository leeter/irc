namespace irc.commands

type public ICommand = 
    interface
    end

type public ConnectToServerAsClientCommand(nickName:string, password:string, userMode:int, userName:string, realName:string) = struct
    member this.NickName = nickName
    member this.Password = password
    member this.UserMode = userMode
    member this.UserName = userName
    member this.RealName = realName
    interface ICommand
end