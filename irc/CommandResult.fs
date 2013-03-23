namespace irc.framework

open irc.enums

type public CommandResult<'responseType>(succeeded:bool, result:CommandResult, responseValue:'responseType) = class
    member this.Succeeded = succeeded
    member this.Result = result
    member this.Value = responseValue
end

