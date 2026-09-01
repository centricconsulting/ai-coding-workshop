package com.example.taskmanager.infrastructure

/**
 * Legacy workshop code for Lab 3.
 *
 * The code works, but it treats Kotlin like nullable Java and repeats
 * manual null checks instead of using the language's null-safety features.
 */
class LegacyTaskProcessor {
    fun processUpdate(request: LegacyTaskUpdateRequest?): String {
        var summary = ""

        if (request != null) {
            if (request.taskId != null) {
                summary = "Task " + request.taskId
            } else {
                summary = "Task unknown"
            }

            if (request.title != null) {
                if (request.title.trim().isNotEmpty()) {
                    summary = summary + " | title=" + request.title.trim()
                } else {
                    summary = summary + " | title=(blank)"
                }
            } else {
                summary = summary + " | title=(missing)"
            }

            if (request.description != null) {
                if (request.description.trim().isNotEmpty()) {
                    summary = summary + " | description=" + request.description.trim()
                } else {
                    summary = summary + " | description=(blank)"
                }
            } else {
                summary = summary + " | description=(missing)"
            }

            if (request.estimateMinutes != null) {
                if (request.estimateMinutes > 0) {
                    summary = summary + " | estimate=" + request.estimateMinutes + "m"
                } else {
                    summary = summary + " | estimate=unplanned"
                }
            } else {
                summary = summary + " | estimate=unknown"
            }

            if (request.assignee != null) {
                if (request.assignee.trim().isNotEmpty()) {
                    summary = summary + " | assignee=" + request.assignee.trim()
                } else {
                    summary = summary + " | assignee=unassigned"
                }
            } else {
                summary = summary + " | assignee=unassigned"
            }

            if (request.status != null) {
                if (request.status == "DONE") {
                    summary = summary + " | state=complete"
                } else {
                    if (request.status == "IN_PROGRESS") {
                        summary = summary + " | state=active"
                    } else {
                        summary = summary + " | state=open"
                    }
                }
            } else {
                summary = summary + " | state=open"
            }

            if (request.notifyChannel != null) {
                if (request.notifyRecipient != null) {
                    if (request.notifyRecipient.trim().isNotEmpty()) {
                        summary =
                            summary +
                                " | notify=" +
                                request.notifyChannel +
                                ":" +
                                request.notifyRecipient.trim()
                    } else {
                        summary = summary + " | notify=skipped"
                    }
                } else {
                    summary = summary + " | notify=skipped"
                }
            } else {
                summary = summary + " | notify=skipped"
            }
        }

        return summary
    }
}

data class LegacyTaskUpdateRequest(
    val taskId: String?,
    val title: String?,
    val description: String?,
    val estimateMinutes: Int?,
    val assignee: String?,
    val status: String?,
    val notifyChannel: String?,
    val notifyRecipient: String?,
)
