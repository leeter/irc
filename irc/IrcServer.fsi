namespace irc.protocol
open System.Threading.Tasks
  type IrcServer =
    class
      interface System.IDisposable
      /// <summary>Instantiates a new instance of <see href="IrcServer"/></summary>
      /// <parameter name="serverAddresses">An <see cref="System.Collections.Generic.IEnumerable{T}"/> of <see cref="System.Net.IPAddress"/>s that correspond to the server</parameter>
      new : serverAddresses:System.Collections.Generic.IEnumerable<string> *
            port:int *
            remoteCertificateValidationCallback:System.Net.Security.RemoteCertificateValidationCallback * 
            userCertificateSelectionCallback:System.Net.Security.LocalCertificateSelectionCallback * 
            encryptionPolicy:System.Net.Security.EncryptionPolicy * 
            encoding:System.Text.Encoding -> IrcServer
      new : serverAddress:string * port:int -> IrcServer
      [<CLIEvent>]
      member MessageRecievedEvent : IEvent<MessageRecievedEventArgs>
      /// <summary>Sends a message to the server</summary>
      /// <parameter name="message">The message to send, should be no longer than 510 characters</parameter>
      member WriteMessage : message:string -> unit
    end

