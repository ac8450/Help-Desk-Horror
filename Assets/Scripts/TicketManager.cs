using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TicketManager : MonoBehaviour
{
    [Header("Ticekt Panel")]
    public GameObject TicketPanel;

    public TMP_Text NameText;
    public TMP_Text DOBText;
    public TMP_Text Title;
    public TMP_Text StudentID;
    public TMP_Text Username;
    public TMP_Text Department;
    public TMP_Text GraduationClass;
    public TMP_Text UniEmail;
    public TMP_Text AltEmail;
    public TMP_Text PhoneNumber;
    public TMP_Text TicketDescription;

    [Header("Answer Panel")]
    public GameObject AnswerPanel;


    [Header("Queue Buttons")]
   public GameObject[] queueButtons;
   public TMP_Text[] queueButtonTexts;

   [Header("Other UI")]
   public TMP_Text dialogueText;
   public TMP_Text clockText;

    private List<Ticket> ticketQueue = new List<Ticket>();

    private int currentOpenTicketIndex = -1;
    private int correctCount = 0;
    private int wrongCount = 0;

    //Ticket queue
    private float nextTicketArrivalTime;
    private int nextTicketIndex = 0;
    private float minDelay = 25f;
    private float maxDelay = 75f;

    private float shiftTimer;

    //ClockTimer
    private float  clockTimer = 0f;
    private float secondsPerHour = 60f;
    private int currentHour = 4;
    private int endHour = 9;

    //Shift state
    private bool shiftActive = true;
    private bool shiftComplete = false;
    private Ticket[] tickets =
    {
        new Ticket(
            "Sarah Miller",
            "Female",
            "9/2/2007",
            "Student",
            "240583",
            "sm0583",
            "Computer Science",
            "2030",
            "sm0583@university.edu",
            "sarah.miller@hmail.com",
            "610-555-1234",
            "Cannot access Microsoft account.",
            "Normal",
            "Help"
            ),
        
        new Ticket(
            "Michael Reed",
            "Male",
            "1/22/2005",
            "Student",
            "184520",
            "mr4520",
            "Criminal Justice",
            "2010",
            "mr4520@university.edu",
            "mikereed29@hmail.com",
            "507-246-6870",
            "I can't find my account anymore.",
            "Anomaly",
            "Reject"
        ),

        new Ticket(
            "Jamie Carter",
            "Male",
            "11/24/2006",
            "Student",
            "241092",
            "jc1092",
            "History",
            "2029",
            "jc1092@university.edu",
            "jamieCarter@hmail.com",
            "417-848-7561",
            "The printer in the lab is out of paper.",
            "Escalation",
            "Help"
        ),

        new Ticket (
            "Justin Ford",
            "Male",
            "12/8/2004",
            "Student",
            "423853",
            "Business Management",
            "2027",
            "jf3853",
            "jf3853@university.edu",
            "fordjustin@hmail.com",
            "472-248-5122",
            "School website is not working.",
            "Normal",
            "Help"
        ),

        new Ticket (
            "Jasmyn Towns",
            "Female",
            "9/4/2006",
            "Student",
            "592546",
            "Nursing",
            "2029",
            "jt2546",
            "jt2546@university.edu",
            "jazzyT@hmail.com",
            "214-586-2932",
            "I need my account password reset",
            "Normal",
            "Help"
        )
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TicketPanel.SetActive(false);
        AnswerPanel.SetActive(false);

        UpdateClock();
        RefreshQueueButtons();
        ScheduleNextTicket();
    }
    // Update is called once per frame
    void Update()
    {
        if (!shiftActive)
        {
            return;
        }

        // increase shift timer
        shiftTimer += Time.deltaTime;
        clockTimer += Time.deltaTime;


        // check if next ticket should arrive
        if (shiftTimer >= nextTicketArrivalTime)
        {
           AddNextTicketToQueue();
        }    

        
        if (clockTimer >= secondsPerHour)
        {
            clockTimer = 0f;
            currentHour++;
            UpdateClock();

            if (currentHour >= endHour)
            {
                shiftActive = false;
                shiftComplete = true;
                ShowDialogue("That's the end of my shift. I should log off.");
            }
        }

        if (TicketPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                HelpTicket();
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                RejectTicket();
            }
        }
    }

    void UpdateClock()
    {
        clockText.text = currentHour + ":00 PM";
    }

    private void ScheduleNextTicket()
    {
        if (nextTicketIndex >= tickets.Length)
        {
            return;
        }
        // generate random delay
        float randomDelay = Random.Range(minDelay, maxDelay);

        // set nextTicketArrivalTime
        nextTicketArrivalTime = shiftTimer + randomDelay;
    }

    private void AddNextTicketToQueue()
    {
        // use nextTicketIndex
        if (nextTicketIndex >= tickets.Length)
        {
            return;
        }
        // add ticket to queue
        ticketQueue.Add(tickets[nextTicketIndex]);
        nextTicketIndex++;

        RefreshQueueButtons();

        if (nextTicketIndex < tickets.Length)
        {
            ScheduleNextTicket();
        }

    }

    private void RefreshQueueButtons()
    {
        for (int i = 0; i < queueButtons.Length; i++)
        {
            if (i < ticketQueue.Count)
            {
                queueButtons[i].SetActive(true);
                queueButtonTexts[i].text = ticketQueue[i].name + "-" + ticketQueue[i].issue;
            }
            else
            {
                queueButtons[i].SetActive(false);
                queueButtonTexts[i].text = "";
            }
        }
    }

    public void OpenQueueTicket0() { OpenTicket(0); }
    public void OpenQueueTicket1() { OpenTicket(1); }
    public void OpenQueueTicket2() { OpenTicket(2); }
    public void OpenQueueTicket3() { OpenTicket(3); }
    public void OpenQueueTicket4() { OpenTicket(4); }
    public void OpenQueueTicket5() { OpenTicket(5); }
    public void OpenQueueTicket6() { OpenTicket(6); }
    public void OpenQueueTicket7() { OpenTicket(7); }
    public void OpenQueueTicket8() { OpenTicket(8); }
    public void OpenQueueTicket9() { OpenTicket(9); }
    private void OpenTicket(int index)
    {
        if (index < 0 || index >= ticketQueue.Count)
        {
            return;
        }

        currentOpenTicketIndex = index;
        TicketPanel.SetActive(true);
        AnswerPanel.SetActive(true);
        ShowCurrentTicket(ticketQueue[index]);
    }
    void ShowCurrentTicket(Ticket ticket)
    {
        NameText.text = "Name: " + ticket.name;
        DOBText.text = "Date of Birth: " + ticket.DOB;
        Title.text = "Title: " + ticket.title;
        StudentID.text = "ID: " + ticket.id;
        Username.text = "Username: " + ticket.username;

        if (ticket.title == "Student")
        {
            Department.text = "Major: " + ticket.department;
        }
        else
        {
            Department.text = "Department: " + ticket.department;
        }

        GraduationClass.text = "Graduation Class: " + ticket.gradClass;
        UniEmail.text = "Email: " + ticket.email;
        AltEmail.text = "Alternate Email: " + ticket.altEmail;
        PhoneNumber.text = "Phone Number: " + ticket.phoneNum;
        TicketDescription.text = ticket.issue;
    }

    void HandleChoice(string playerChoice)
    {
        if (currentOpenTicketIndex < 0 || currentOpenTicketIndex >= ticketQueue.Count)
        {
            return;
        }

        Ticket ticket = ticketQueue[currentOpenTicketIndex];

        if (playerChoice == ticket.correctAction)
        {
            correctCount++;
            Debug.Log("Correct choice");
        }
        else
        {
            wrongCount++;
            Debug.Log("Wrong choice");
        }

        ResolveCurrentTicket();
    }
    void ResolveCurrentTicket()
    {
        if (currentOpenTicketIndex < 0 || currentOpenTicketIndex >= ticketQueue.Count)
        {
            return;
        }

        ticketQueue.RemoveAt(currentOpenTicketIndex);

        currentOpenTicketIndex = -1;
        TicketPanel.SetActive(false);
        AnswerPanel.SetActive(false);

        RefreshQueueButtons();
    }

    public void ShowDialogue(string line)
    {
        dialogueText.text = line;
        CancelInvoke(nameof(ClearDialogue));
        Invoke(nameof(ClearDialogue), 3f);
    }
    public void ClearDialogue()
    {
        dialogueText.text = "";
    }

    // Add to computerpanel script in future
    public void SignOut()
    {
        if (!shiftComplete)
        {
            ShowDialogue("I can't leave yet. My shift ends at 9.");
            return;
        }
        ShowDialogue("Shift complete.");
        Debug.Log("Sign out clicked");
    }
    public void HelpTicket()
    {
        HandleChoice("Help");
    }

    public void RejectTicket()
    {
         HandleChoice("Reject");
    }

}

public class Ticket
{
    public string name;
    public string gender;
    public string DOB;
    public string title;
    public string id;
    public string username;
    public string department;
    public string gradClass;
    public string email;
    public string altEmail;
    public string phoneNum;
    public string issue;
    public string category;
    public string correctAction;

    public Ticket(string name, string gender, string DOB, string title, string id, string username, 
        string department, string gradClass, string email, string altEmail, string phoneNum,
        string issue, string category, string correctAction)
    {
        this.name  = name;
        this.gender = gender;
        this.DOB = DOB;
        this.title = title;
        this.id = id;
        this.username = username;
        this.department = department;
        this.gradClass = gradClass;
        this.email = email;
        this.altEmail = altEmail;
        this.phoneNum = phoneNum;
        this.issue = issue;
        this.category = category;
        this.correctAction = correctAction;
    }
}
