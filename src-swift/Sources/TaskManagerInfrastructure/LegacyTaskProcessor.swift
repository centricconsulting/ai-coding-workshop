import Foundation

/// Legacy workshop code for Lab 3.
///
/// The code works, but it force-unwraps optionals and builds ad-hoc output
/// strings instead of using safer Swift patterns.
public final class LegacyTaskProcessor {
    public init() {}

    public func processUpdate(_ request: LegacyTaskUpdateRequest?) -> String {
        var summary = ""

        if request != nil {
            if request!.taskID != nil {
                summary = "Task \(request!.taskID!)"
            } else {
                summary = "Task unknown"
            }

            if request!.title != nil {
                if request!.title!.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty {
                    summary += " | title=(blank)"
                } else {
                    summary += " | title=\(request!.title!.trimmingCharacters(in: .whitespacesAndNewlines))"
                }
            } else {
                summary += " | title=(missing)"
            }

            if request!.description != nil {
                if request!.description!.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty {
                    summary += " | description=(blank)"
                } else {
                    summary += " | description=\(request!.description!.trimmingCharacters(in: .whitespacesAndNewlines))"
                }
            } else {
                summary += " | description=(missing)"
            }

            if request!.estimateMinutes != nil {
                if request!.estimateMinutes! > 0 {
                    summary += " | estimate=\(request!.estimateMinutes!)m"
                } else {
                    summary += " | estimate=unplanned"
                }
            } else {
                summary += " | estimate=unknown"
            }

            if request!.assignee != nil {
                if request!.assignee!.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty {
                    summary += " | assignee=unassigned"
                } else {
                    summary += " | assignee=\(request!.assignee!.trimmingCharacters(in: .whitespacesAndNewlines))"
                }
            } else {
                summary += " | assignee=unassigned"
            }

            if request!.status != nil {
                if request!.status! == "DONE" {
                    summary += " | state=complete"
                } else if request!.status! == "IN_PROGRESS" {
                    summary += " | state=active"
                } else {
                    summary += " | state=open"
                }
            } else {
                summary += " | state=open"
            }

            if request!.notifyChannel != nil {
                if request!.notifyRecipient != nil {
                    if request!.notifyRecipient!.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty {
                        summary += " | notify=skipped"
                    } else {
                        summary += " | notify=\(request!.notifyChannel!):\(request!.notifyRecipient!.trimmingCharacters(in: .whitespacesAndNewlines))"
                    }
                } else {
                    summary += " | notify=skipped"
                }
            } else {
                summary += " | notify=skipped"
            }
        }

        return summary
    }
}

public struct LegacyTaskUpdateRequest {
    public let taskID: String?
    public let title: String?
    public let description: String?
    public let estimateMinutes: Int?
    public let assignee: String?
    public let status: String?
    public let notifyChannel: String?
    public let notifyRecipient: String?

    public init(
        taskID: String?,
        title: String?,
        description: String?,
        estimateMinutes: Int?,
        assignee: String?,
        status: String?,
        notifyChannel: String?,
        notifyRecipient: String?
    ) {
        self.taskID = taskID
        self.title = title
        self.description = description
        self.estimateMinutes = estimateMinutes
        self.assignee = assignee
        self.status = status
        self.notifyChannel = notifyChannel
        self.notifyRecipient = notifyRecipient
    }
}
