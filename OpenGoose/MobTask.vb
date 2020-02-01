Public MustInherit Class MobTask
    Public Enum TaskState
        NotBegun
        Running
        Completed
        Errored
        Aborted
    End Enum

    Private mMob As DesktopMob
    Public ReadOnly Property Mob As DesktopMob
        Get
            Return mMob
        End Get
    End Property

    Private mState As TaskState = TaskState.NotBegun
    Public ReadOnly Property State As TaskState
        Get
            Return mState
        End Get
    End Property

    Public ReadOnly Property IsStopped As Boolean
        Get
            Return (mState = TaskState.Completed) Or (mState = TaskState.Errored) Or (mState = TaskState.Aborted)
        End Get
    End Property

    Public ReadOnly Property Weight As Integer
        Get
            Return TaskWeightAttribute.GetWeight(Me.GetType)
        End Get
    End Property

    Public Event Finished(taskSender As MobTask)
    Public Event UnhandledException(taskSender As MobTask, ex As Exception)

    Public Sub New(mob As DesktopMob)
        mMob = mob
    End Sub

    Protected MustOverride Sub OnBegin()

    Public Sub Begin()
        OnBegin()
        mState = TaskState.Running
    End Sub

    Protected MustOverride Sub OnTick()

    Public Sub Tick()
        Try
            OnTick()
        Catch ex As Exception
            mState = TaskState.Errored
            RaiseEvent UnhandledException(Me, ex)
        End Try
    End Sub

    Protected Overridable Sub OnAbort()
        SetAborted()
    End Sub

    Public Sub Abort()
        Try
            OnAbort()
        Catch ex As Exception
            mState = TaskState.Errored
            RaiseEvent UnhandledException(Me, ex)
        End Try
    End Sub

    Protected Sub SetComplete()
        If mState = TaskState.Running Then
            mState = TaskState.Completed
        Else
            Throw New InvalidOperationException("Cannot set non-running task to complete")
        End If
    End Sub

    Protected Sub SetAborted()
        If mState = TaskState.Running Then
            mState = TaskState.Aborted
        Else
            Throw New InvalidOperationException("Cannot set non-running task to aborted")
        End If
    End Sub
End Class
